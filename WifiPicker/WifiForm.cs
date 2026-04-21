using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace WifiPicker
{
	public partial class WifiForm : Form
	{
		[GeneratedRegex(@"(?:All User Profile|Current User Profile|Perfil de todos los usuarios|Perfil de usuario actual)\s*:\s*(.+)")]
		private static partial Regex ProfilesRegex();

		[GeneratedRegex(@"^\s{1,10}SSID\s*:\s*(.+)$")]
		private static partial Regex CurrentSsidRegex();

		[GeneratedRegex(@"SSID \d+ :")]
		private static partial Regex SsidBlockRegex();

		[GeneratedRegex(@"Signal\s*:\s*(\d+)%|Señal\s*:\s*(\d+)%")]
		private static partial Regex SignalRegex();

		private List<WifiNetwork> _networks = new();
		private List<string> _profiles = new();
		private string _currentSSID = "";
		private System.Windows.Forms.Timer _timer = new();

		public WifiForm()
		{
			InitializeComponent();
			SetupListView();
			WireEvents();
			LoadIcon();
			_ = InitAsync();
		}

		private void LoadIcon()
		{
			using var stream = typeof(WifiForm).Assembly
				.GetManifestResourceStream("WifiPicker.wifi.ico");
			if (stream != null)
				this.Icon = new Icon(stream);
		}

		// Columnas aquí para que el Designer de VS no las borre
		private void SetupListView()
		{
			lvNetworks.Columns.Add("Red (SSID)", 220);
			lvNetworks.Columns.Add("Señal", 180);
			lvNetworks.Columns.Add("Seguridad", 135);
			lvNetworks.Columns.Add("Guardada", 150);
			lvNetworks.HideSelection = false;
			lblConnectHint.AutoSize = false;
		}

		// ────────────────────────────────────────────────────────────
		//  Wiring
		// ────────────────────────────────────────────────────────────
		private void WireEvents()
		{
			btnRefresh.Click += async (_, _) => await RefreshAsync();
			btnConnect.Click += async (_, _) => await ConnectAsync();
			btnDisconnect.Click += async (_, _) => await DisconnectAsync();
			OlvidartBtn.Click += async (_, _) => await OlvidarRedAsync();

			chkShowPass.CheckedChanged += (_, _) =>
				txtPassword.UseSystemPasswordChar = !chkShowPass.Checked;

			txtPassword.KeyDown += (_, e) =>
			{
				if (e.KeyCode == Keys.Enter) btnConnect.PerformClick();
			};

			lvNetworks.SelectedIndexChanged += OnNetworkSelected;
			lvNetworks.DoubleClick += async (_, _) => await ConnectAsync();

			_timer.Interval = 20_000;   // auto-refresh every 20 s
			_timer.Tick += async (_, _) => await RefreshAsync();
			_timer.Start();
		}

		private async Task InitAsync() => await RefreshAsync();

		// ────────────────────────────────────────────────────────────
		//  Refresh
		// ────────────────────────────────────────────────────────────
		private async Task RefreshAsync()
		{
			SetStatus("Buscando redes...");
			btnRefresh.Enabled = false;
			try
			{
				_profiles = await GetProfilesAsync();
				_networks = await ScanAsync();
				_currentSSID = await GetCurrentSSIDAsync();
				UpdateList();
				UpdateTopBar();
			}
			catch (Exception ex)
			{
				SetStatus($"Error: {ex.Message}");
			}
			finally
			{
				btnRefresh.Enabled = true;
			}
		}

		// ────────────────────────────────────────────────────────────
		//  netsh helpers
		// ────────────────────────────────────────────────────────────
		private static async Task<string> RunAsync(string args)
		{
			var psi = new ProcessStartInfo("cmd.exe", $"/c chcp 65001 > nul & {args}")
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				StandardOutputEncoding = Encoding.UTF8
			};
			using var p = Process.Start(psi)!;
			string output = await p.StandardOutput.ReadToEndAsync();
			await p.WaitForExitAsync();
			return output;
		}

		private async Task<List<string>> GetProfilesAsync()
		{
			string raw = await RunAsync("netsh wlan show profiles");
			return ProfilesRegex().Matches(raw)
						.Cast<Match>()
						.Select(m => m.Groups[1].Value.Trim())
						.ToList();
		}

		private async Task<string> GetCurrentSSIDAsync()
		{
			string raw = await RunAsync("netsh wlan show interfaces");
			foreach (string line in raw.Split('\n'))
			{
				var m = CurrentSsidRegex().Match(line);
				if (m.Success)
					return m.Groups[1].Value.Trim();
			}
			return "";
		}

		private async Task<List<WifiNetwork>> ScanAsync()
		{
			// Fuerza un nuevo escaneo — si no, Windows devuelve el caché
			// y estando conectado nunca aparecen redes nuevas.
			// netsh wlan scan vuelve al instante, pero el scan real dura
			// 3-6s (más cuando estamos conectados). Hacemos polling: leemos
			// la lista cada segundo hasta que el conteo se estabilice.
			await RunAsync("netsh wlan scan");
			await Task.Delay(1500); // mínimo razonable

			string raw = await RunAsync("netsh wlan show networks mode=bssid");
			int lastCount = SsidBlockRegex().Matches(raw).Count;

			for (int i = 0; i < 5; i++) // hasta ~5s extra
			{
				await Task.Delay(1000);
				string next = await RunAsync("netsh wlan show networks mode=bssid");
				int count = SsidBlockRegex().Matches(next).Count;
				raw = next;
				if (count == lastCount && count > 0) break; // estable → scan terminó
				lastCount = count;
			}

			var result = new List<WifiNetwork>();

			var blocks = SsidBlockRegex().Split(raw).Skip(1).ToArray();

			foreach (string block in blocks)
			{
				var lines = block.Split('\n');
				string ssid = lines[0].Trim();
				if (string.IsNullOrWhiteSpace(ssid)) continue;

				string auth = Val(block, "Authentication|Autenticación");
				int signal = SignalRegex().Matches(block)
									.Cast<Match>()
									.Select(m =>
									{
										var g = m.Groups[1].Success ? m.Groups[1] : m.Groups[2];
										return g.Success ? int.Parse(g.Value) : 0;
									})
									.DefaultIfEmpty(0)
									.Max();

				// Merge duplicate SSIDs, keep max signal
				var existing = result.FirstOrDefault(n => n.SSID == ssid);
				if (existing != null)
				{
					if (signal > existing.Signal) existing.Signal = signal;
				}
				else
				{
					result.Add(new WifiNetwork
					{
						SSID = ssid,
						Auth = NormalizeAuth(auth),
						Signal = signal,
						HasProfile = _profiles.Contains(ssid)
					});
				}
			}

			return result.OrderByDescending(n => n.Signal).ToList();
		}

		// ────────────────────────────────────────────────────────────
		//  Connect / Disconnect
		// ────────────────────────────────────────────────────────────
		private async Task ConnectAsync()
		{
			if (lvNetworks.SelectedItems.Count == 0) return;
			var net = (WifiNetwork)lvNetworks.SelectedItems[0].Tag!;
			if (net.IsConnected) return;

			// Enterprise auth — can't connect via simple password
			if (net.Auth == WifiAuth.Enterprise)
			{
				MessageBox.Show(
					"Esta red usa autenticación empresarial (WPA2/WPA3-Enterprise).\n" +
					"Debe configurarse manualmente con credenciales corporativas.",
					"Red empresarial", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			// Password required but not provided
			bool needsPassword = net.Auth != WifiAuth.Open && !net.HasProfile;
			if (needsPassword && txtPassword.Text.Length < 8)
			{
				MessageBox.Show("La contraseña debe tener al menos 8 caracteres.",
					"Contraseña incorrecta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				txtPassword.Focus();
				return;
			}

			btnConnect.Enabled = false;
			SetStatus($"Conectando a {net.SSID}...");

			try
			{
				if (!net.HasProfile)
				{
					string password = needsPassword ? txtPassword.Text : "";
					await CreateProfileAsync(net.SSID, password, net.Auth);
				}

				string result = await RunAsync($"netsh wlan connect name=\"{EscCmd(net.SSID)}\"");

				if (result.Contains("error") || result.Contains("Error"))
					throw new Exception(result.Trim());

				SetStatus($"Conectando a {net.SSID}... espere.");
				await Task.Delay(3000);
				await RefreshAsync();
			}
			catch (Exception ex)
			{
				SetStatus($"Error al conectar: {ex.Message}");
				MessageBox.Show($"No se pudo conectar a '{net.SSID}':\n\n{ex.Message}",
					"Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				btnConnect.Enabled = true;
			}
		}

		private async Task DisconnectAsync()
		{
			btnDisconnect.Enabled = false;
			SetStatus("Desconectando...");
			await RunAsync("netsh wlan disconnect");
			await Task.Delay(1500);
			await RefreshAsync();
		}

		private async Task CreateProfileAsync(string ssid, string password, WifiAuth auth)
		{
			string xml = BuildProfileXml(ssid, password, auth);
			string tmp = Path.Combine(Path.GetTempPath(), $"wifiprofile_{Guid.NewGuid()}.xml");
			try
			{
				// Sin BOM para que netsh lo lea correctamente
				await File.WriteAllTextAsync(tmp, xml, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
				string result = await RunAsync($"netsh wlan add profile filename=\"{tmp}\" user=current");

				// El perfil se agrega correctamente si el output contiene el SSID seguido de "added" o "agregado"
				bool success = result.Contains(ssid) || result.Contains("added") || result.Contains("agregado")
				               || result.Contains("añadido") || result.Contains("agregó");
				bool failed  = result.Contains("error") || result.Contains("Error")
				               || result.Contains("denegado") || result.Contains("denied")
				               || result.Contains("no se") || result.Contains("cannot");

				if (!success || failed)
					throw new Exception($"No se pudo guardar el perfil.\n\nSalida de netsh:\n{result.Trim()}");
			}
			finally
			{
				if (File.Exists(tmp)) File.Delete(tmp);
			}
		}

		private static string BuildProfileXml(string ssid, string password, WifiAuth auth)
		{
			string eName = Xml(ssid);

			if (auth == WifiAuth.Open)
				return $"""
                    <?xml version="1.0"?>
                    <WLANProfile xmlns="http://www.microsoft.com/networking/WLAN/profile/v1">
                        <name>{eName}</name>
                        <SSIDConfig><SSID><name>{eName}</name></SSID></SSIDConfig>
                        <connectionType>ESS</connectionType>
                        <connectionMode>manual</connectionMode>
                        <MSM><security><authEncryption>
                            <authentication>open</authentication>
                            <encryption>none</encryption>
                            <useOneX>false</useOneX>
                        </authEncryption></security></MSM>
                    </WLANProfile>
                    """;

			var authEnc = auth switch
			{
				WifiAuth.WPA3 => ("WPA3SAE", "AES"),
				WifiAuth.WPA => ("WPAPSK", "TKIP"),
				_ => ("WPA2PSK", "AES"),
			};
			string authTag = authEnc.Item1, encTag = authEnc.Item2;

			return $"""
                <?xml version="1.0"?>
                <WLANProfile xmlns="http://www.microsoft.com/networking/WLAN/profile/v1">
                    <name>{eName}</name>
                    <SSIDConfig><SSID><name>{eName}</name></SSID></SSIDConfig>
                    <connectionType>ESS</connectionType>
                    <connectionMode>manual</connectionMode>
                    <MSM><security>
                        <authEncryption>
                            <authentication>{authTag}</authentication>
                            <encryption>{encTag}</encryption>
                            <useOneX>false</useOneX>
                        </authEncryption>
                        <sharedKey>
                            <keyType>passPhrase</keyType>
                            <protected>false</protected>
                            <keyMaterial>{Xml(password)}</keyMaterial>
                        </sharedKey>
                    </security></MSM>
                </WLANProfile>
                """;
		}

		// ────────────────────────────────────────────────────────────
		//  UI updates
		// ────────────────────────────────────────────────────────────
		private void UpdateList()
		{
			// Remember selection
			string? prev = lvNetworks.SelectedItems.Count > 0
				? ((WifiNetwork)lvNetworks.SelectedItems[0].Tag!).SSID
				: null;

			lvNetworks.BeginUpdate();
			lvNetworks.Items.Clear();

			foreach (var net in _networks)
			{
				net.IsConnected = net.SSID == _currentSSID;

				string name = net.IsConnected ? $"✓  {net.SSID}" : $"    {net.SSID}";
				var item = new ListViewItem(name);
				item.SubItems.Add(SignalBars(net.Signal));
				item.SubItems.Add(AuthLabel(net.Auth));
				item.SubItems.Add(net.HasProfile ? "Sí" : "");
				item.Tag = net;

				if (net.IsConnected)
				{
					item.BackColor = Color.FromArgb(218, 248, 220);
					item.Font = new Font(lvNetworks.Font, FontStyle.Bold);
				}
				else if (net.HasProfile)
				{
					item.ForeColor = Color.FromArgb(20, 80, 180);
				}

				lvNetworks.Items.Add(item);
			}

			// Restore selection
			if (prev != null)
			{
				foreach (ListViewItem item in lvNetworks.Items)
				{
					if (((WifiNetwork)item.Tag!).SSID == prev)
					{
						item.Selected = true;
						break;
					}
				}
			}

			lvNetworks.EndUpdate();
			SetStatus($"{_networks.Count} redes encontradas · Actualizado: {DateTime.Now:HH:mm:ss}");
		}

		private void UpdateTopBar()
		{
			if (!string.IsNullOrEmpty(_currentSSID))
			{
				lblCurrentConn.Text = $"Conectado a:  {_currentSSID}";
				lblCurrentConn.ForeColor = Color.White;
				btnDisconnect.Enabled = true;
			}
			else
			{
				lblCurrentConn.Text = "Sin conexión WiFi";
				lblCurrentConn.ForeColor = Color.FromArgb(220, 220, 220);
				btnDisconnect.Enabled = false;
			}
		}

		private void OnNetworkSelected(object? sender, EventArgs e)
		{
			if (lvNetworks.SelectedItems.Count == 0)
			{
				pnlConnect.Visible = false;
				return;
			}

			var net = (WifiNetwork)lvNetworks.SelectedItems[0].Tag!;

			lblSelectedSSID.Text = net.SSID;
			lblSelectedSec.Text = AuthLabel(net.Auth);

			bool needsPassword = net.Auth != WifiAuth.Open && !net.HasProfile;
			bool isEnterprise = net.Auth == WifiAuth.Enterprise;

			lblPassword.Visible = needsPassword && !isEnterprise;
			txtPassword.Visible = needsPassword && !isEnterprise;
			chkShowPass.Visible = needsPassword && !isEnterprise;
			txtPassword.Clear();

			if (net.IsConnected)
				lblConnectHint.Text = "Ya estás conectado a esta red.";
			else if (isEnterprise)
				lblConnectHint.Text = "Red empresarial: se necesitan credenciales corporativas.";
			else if (net.HasProfile)
				lblConnectHint.Text = "Red guardada · no necesita contraseña.";
			else if (net.Auth == WifiAuth.Open)
				lblConnectHint.Text = "Red abierta · sin contraseña.";
			else
				lblConnectHint.Text = "Ingrese la contraseña para conectarse.";

			btnConnect.Enabled = !net.IsConnected && !isEnterprise;
			btnConnect.Text = net.IsConnected ? "Ya conectado" : "Conectar";

			OlvidartBtn.Visible = net.HasProfile;

			pnlConnect.Visible = true;
		}

		private async Task OlvidarRedAsync()
		{
			if (lvNetworks.SelectedItems.Count == 0) return;
			var net = (WifiNetwork)lvNetworks.SelectedItems[0].Tag!;

			var confirm = MessageBox.Show(
				$"¿Olvidar la red \"{net.SSID}\"?\nSe borrará la contraseña guardada.",
				"Olvidar red", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (confirm != DialogResult.Yes) return;

			OlvidartBtn.Enabled = false;
			SetStatus($"Olvidando red {net.SSID}...");

			try
			{
				// Si está conectado a esa red, desconectar primero
				if (net.IsConnected)
					await RunAsync("netsh wlan disconnect");

				await RunAsync($"netsh wlan delete profile name=\"{EscCmd(net.SSID)}\"");
				await RefreshAsync();
			}
			catch (Exception ex)
			{
				SetStatus($"Error: {ex.Message}");
			}
			finally
			{
				OlvidartBtn.Enabled = true;
			}
		}

		// ────────────────────────────────────────────────────────────
		//  Helpers
		// ────────────────────────────────────────────────────────────
		private static string SignalBars(int s)
		{
			string bars = s >= 80 ? "████" :
						  s >= 60 ? "███░" :
						  s >= 40 ? "██░░" :
						  s >= 20 ? "█░░░" :
									"░░░░";
			return $"{bars} {s}%";
		}

		private static string AuthLabel(WifiAuth a) => a switch
		{
			WifiAuth.Open => "Abierta",
			WifiAuth.WPA => "WPA",
			WifiAuth.WPA2 => "WPA2",
			WifiAuth.WPA3 => "WPA3",
			WifiAuth.Enterprise => "WPA2/3-Enterprise",
			_ => "Desconocida"
		};

		private static WifiAuth NormalizeAuth(string raw) =>
			raw switch
			{
				var s when s.Contains("Enterprise") || s.Contains("Empresarial") => WifiAuth.Enterprise,
				var s when s.StartsWith("WPA3") => WifiAuth.WPA3,
				var s when s.StartsWith("WPA2") => WifiAuth.WPA2,
				var s when s.StartsWith("WPA") => WifiAuth.WPA,
				var s when s.Contains("Open") || s.Contains("Abierta") => WifiAuth.Open,
				_ => WifiAuth.WPA2
			};

		private static string Val(string block, string key) =>
			Regex.Match(block, $@"(?:{key})\s*:\s*(.+)", RegexOptions.Multiline)
				 .Groups[1].Value.Trim();

		private static string Xml(string s) => s
			.Replace("&", "&amp;")
			.Replace("<", "&lt;")
			.Replace(">", "&gt;")
			.Replace("\"", "&quot;");

		// Escape double-quotes for cmd.exe
		private static string EscCmd(string s) => s.Replace("\"", "\\\"");

		private void SetStatus(string msg) => lblStatus.Text = msg;

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			_timer.Stop();
			_timer.Dispose();
			base.OnFormClosing(e);
		}

		private void pnlConnect_Paint(object sender, PaintEventArgs e)
		{

		}

		private void chkShowPass_CheckedChanged(object sender, EventArgs e)
		{

		}
	}

	// ────────────────────────────────────────────────────────────────
	//  Model
	// ────────────────────────────────────────────────────────────────
	enum WifiAuth { Open, WPA, WPA2, WPA3, Enterprise }

    class WifiNetwork
    {
        public string   SSID       { get; set; } = "";
        public WifiAuth Auth       { get; set; }
        public int      Signal     { get; set; }
        public bool     HasProfile { get; set; }
        public bool     IsConnected{ get; set; }
    }
}
