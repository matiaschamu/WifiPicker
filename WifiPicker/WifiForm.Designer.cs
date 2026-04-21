namespace WifiPicker
{
    partial class WifiForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

		private void InitializeComponent()
		{
			lvNetworks = new ListView();
			lblCurrentConn = new Label();
			btnDisconnect = new Button();
			btnRefresh = new Button();
			pnlConnect = new Panel();
			OlvidartBtn = new Button();
			lblPanelTitle = new Label();
			sep = new Panel();
			lblSSIDTitle = new Label();
			lblSelectedSSID = new Label();
			lblSecTitle = new Label();
			lblSelectedSec = new Label();
			lblPassword = new Label();
			txtPassword = new TextBox();
			chkShowPass = new CheckBox();
			lblConnectHint = new Label();
			btnConnect = new Button();
			lblStatus = new Label();
			lblNetHeader = new Label();
			pnlTop = new Panel();
			pnlSeparator = new Panel();
			pnlConnect.SuspendLayout();
			pnlTop.SuspendLayout();
			SuspendLayout();
			// 
			// lvNetworks
			// 
			lvNetworks.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			lvNetworks.BorderStyle = BorderStyle.FixedSingle;
			lvNetworks.Font = new Font("Segoe UI", 9.5F);
			lvNetworks.FullRowSelect = true;
			lvNetworks.Location = new Point(11, 137);
			lvNetworks.MultiSelect = false;
			lvNetworks.Name = "lvNetworks";
			lvNetworks.Size = new Size(709, 438);
			lvNetworks.TabIndex = 4;
			lvNetworks.UseCompatibleStateImageBehavior = false;
			lvNetworks.View = View.Details;
			// 
			// lblCurrentConn
			// 
			lblCurrentConn.AutoSize = true;
			lblCurrentConn.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			lblCurrentConn.ForeColor = Color.White;
			lblCurrentConn.Location = new Point(14, 13);
			lblCurrentConn.Name = "lblCurrentConn";
			lblCurrentConn.Size = new Size(317, 38);
			lblCurrentConn.TabIndex = 0;
			lblCurrentConn.Text = "Verificando conexión...";
			// 
			// btnDisconnect
			// 
			btnDisconnect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnDisconnect.AutoSize = true;
			btnDisconnect.BackColor = Color.FromArgb(190, 45, 45);
			btnDisconnect.Cursor = Cursors.Hand;
			btnDisconnect.Enabled = false;
			btnDisconnect.FlatAppearance.BorderColor = Color.FromArgb(190, 45, 45);
			btnDisconnect.FlatStyle = FlatStyle.Flat;
			btnDisconnect.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			btnDisconnect.ForeColor = Color.White;
			btnDisconnect.Location = new Point(887, 12);
			btnDisconnect.Name = "btnDisconnect";
			btnDisconnect.Size = new Size(154, 42);
			btnDisconnect.TabIndex = 1;
			btnDisconnect.Text = "Desconectar";
			btnDisconnect.UseVisualStyleBackColor = false;
			// 
			// btnRefresh
			// 
			btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnRefresh.AutoSize = true;
			btnRefresh.BackColor = Color.FromArgb(25, 90, 185);
			btnRefresh.Cursor = Cursors.Hand;
			btnRefresh.FlatAppearance.BorderColor = Color.FromArgb(25, 90, 185);
			btnRefresh.FlatStyle = FlatStyle.Flat;
			btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			btnRefresh.ForeColor = Color.White;
			btnRefresh.Location = new Point(887, 76);
			btnRefresh.Name = "btnRefresh";
			btnRefresh.Size = new Size(154, 45);
			btnRefresh.TabIndex = 3;
			btnRefresh.Text = "Actualizar";
			btnRefresh.UseVisualStyleBackColor = false;
			// 
			// pnlConnect
			// 
			pnlConnect.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			pnlConnect.BackColor = Color.White;
			pnlConnect.BorderStyle = BorderStyle.FixedSingle;
			pnlConnect.Controls.Add(OlvidartBtn);
			pnlConnect.Controls.Add(lblPanelTitle);
			pnlConnect.Controls.Add(sep);
			pnlConnect.Controls.Add(lblSSIDTitle);
			pnlConnect.Controls.Add(lblSelectedSSID);
			pnlConnect.Controls.Add(lblSecTitle);
			pnlConnect.Controls.Add(lblSelectedSec);
			pnlConnect.Controls.Add(lblPassword);
			pnlConnect.Controls.Add(txtPassword);
			pnlConnect.Controls.Add(chkShowPass);
			pnlConnect.Controls.Add(lblConnectHint);
			pnlConnect.Controls.Add(btnConnect);
			pnlConnect.Location = new Point(724, 137);
			pnlConnect.Name = "pnlConnect";
			pnlConnect.Size = new Size(320, 438);
			pnlConnect.TabIndex = 5;
			pnlConnect.Visible = false;
			// 
			// OlvidartBtn
			// 
			OlvidartBtn.AutoSize = true;
			OlvidartBtn.BackColor = Color.FromArgb(190, 90, 10);
			OlvidartBtn.Cursor = Cursors.Hand;
			OlvidartBtn.FlatAppearance.BorderColor = Color.FromArgb(170, 75, 5);
			OlvidartBtn.FlatStyle = FlatStyle.Flat;
			OlvidartBtn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
			OlvidartBtn.ForeColor = Color.White;
			OlvidartBtn.Location = new Point(3, 366);
			OlvidartBtn.Name = "OlvidartBtn";
			OlvidartBtn.Size = new Size(312, 50);
			OlvidartBtn.TabIndex = 11;
			OlvidartBtn.Text = "Olvidar red";
			OlvidartBtn.UseVisualStyleBackColor = false;
			// 
			// lblPanelTitle
			// 
			lblPanelTitle.AutoSize = true;
			lblPanelTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
			lblPanelTitle.ForeColor = Color.FromArgb(25, 90, 185);
			lblPanelTitle.Location = new Point(3, 0);
			lblPanelTitle.Name = "lblPanelTitle";
			lblPanelTitle.Size = new Size(191, 36);
			lblPanelTitle.TabIndex = 0;
			lblPanelTitle.Text = "Conectar a red";
			// 
			// sep
			// 
			sep.BackColor = Color.FromArgb(220, 225, 240);
			sep.Location = new Point(0, 43);
			sep.Name = "sep";
			sep.Size = new Size(295, 1);
			sep.TabIndex = 1;
			// 
			// lblSSIDTitle
			// 
			lblSSIDTitle.Font = new Font("Segoe UI", 8F);
			lblSSIDTitle.ForeColor = Color.Gray;
			lblSSIDTitle.Location = new Point(3, 113);
			lblSSIDTitle.Name = "lblSSIDTitle";
			lblSSIDTitle.Size = new Size(250, 32);
			lblSSIDTitle.TabIndex = 2;
			lblSSIDTitle.Text = "Red seleccionada:";
			// 
			// lblSelectedSSID
			// 
			lblSelectedSSID.AutoEllipsis = true;
			lblSelectedSSID.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			lblSelectedSSID.ForeColor = Color.FromArgb(20, 20, 20);
			lblSelectedSSID.Location = new Point(3, 142);
			lblSelectedSSID.Name = "lblSelectedSSID";
			lblSelectedSSID.Size = new Size(209, 46);
			lblSelectedSSID.TabIndex = 3;
			lblSelectedSSID.Text = "dddddd";
			// 
			// lblSecTitle
			// 
			lblSecTitle.Font = new Font("Segoe UI", 8F);
			lblSecTitle.ForeColor = Color.Gray;
			lblSecTitle.Location = new Point(3, 198);
			lblSecTitle.Name = "lblSecTitle";
			lblSecTitle.Size = new Size(188, 32);
			lblSecTitle.TabIndex = 4;
			lblSecTitle.Text = "Seguridad:";
			// 
			// lblSelectedSec
			// 
			lblSelectedSec.Font = new Font("Segoe UI", 9F);
			lblSelectedSec.ForeColor = Color.FromArgb(30, 30, 30);
			lblSelectedSec.Location = new Point(217, 151);
			lblSelectedSec.Name = "lblSelectedSec";
			lblSelectedSec.Size = new Size(98, 34);
			lblSelectedSec.TabIndex = 5;
			lblSelectedSec.Text = "dddddd";
			// 
			// lblPassword
			// 
			lblPassword.AutoSize = true;
			lblPassword.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
			lblPassword.ForeColor = Color.FromArgb(35, 35, 35);
			lblPassword.Location = new Point(3, 230);
			lblPassword.Name = "lblPassword";
			lblPassword.Size = new Size(129, 30);
			lblPassword.TabIndex = 6;
			lblPassword.Text = "Contraseña:";
			// 
			// txtPassword
			// 
			txtPassword.BorderStyle = BorderStyle.FixedSingle;
			txtPassword.Font = new Font("Segoe UI", 10.5F);
			txtPassword.Location = new Point(3, 262);
			txtPassword.Name = "txtPassword";
			txtPassword.Size = new Size(233, 40);
			txtPassword.TabIndex = 7;
			txtPassword.UseSystemPasswordChar = true;
			// 
			// chkShowPass
			// 
			chkShowPass.AutoSize = true;
			chkShowPass.Font = new Font("Segoe UI", 9F);
			chkShowPass.Location = new Point(241, 268);
			chkShowPass.Name = "chkShowPass";
			chkShowPass.Size = new Size(69, 34);
			chkShowPass.TabIndex = 8;
			chkShowPass.Text = "Ver";
			chkShowPass.CheckedChanged += chkShowPass_CheckedChanged;
			// 
			// lblConnectHint
			// 
			lblConnectHint.Font = new Font("Segoe UI", 8.5F);
			lblConnectHint.ForeColor = Color.FromArgb(100, 105, 115);
			lblConnectHint.Location = new Point(3, 308);
			lblConnectHint.Name = "lblConnectHint";
			lblConnectHint.Size = new Size(312, 52);
			lblConnectHint.TabIndex = 9;
			// 
			// btnConnect
			// 
			btnConnect.AutoSize = true;
			btnConnect.BackColor = Color.FromArgb(28, 155, 50);
			btnConnect.Cursor = Cursors.Hand;
			btnConnect.FlatAppearance.BorderColor = Color.FromArgb(28, 155, 50);
			btnConnect.FlatStyle = FlatStyle.Flat;
			btnConnect.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
			btnConnect.ForeColor = Color.White;
			btnConnect.Location = new Point(3, 50);
			btnConnect.Name = "btnConnect";
			btnConnect.Size = new Size(312, 50);
			btnConnect.TabIndex = 10;
			btnConnect.Text = "Conectar";
			btnConnect.UseVisualStyleBackColor = false;
			// 
			// lblStatus
			// 
			lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			lblStatus.BackColor = Color.FromArgb(238, 240, 245);
			lblStatus.BorderStyle = BorderStyle.Fixed3D;
			lblStatus.Font = new Font("Segoe UI", 8.5F);
			lblStatus.ForeColor = Color.FromArgb(70, 75, 90);
			lblStatus.Location = new Point(11, 581);
			lblStatus.Name = "lblStatus";
			lblStatus.Padding = new Padding(9, 4, 0, 0);
			lblStatus.Size = new Size(1034, 35);
			lblStatus.TabIndex = 6;
			lblStatus.Text = "Iniciando...";
			// 
			// lblNetHeader
			// 
			lblNetHeader.AutoSize = true;
			lblNetHeader.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
			lblNetHeader.ForeColor = Color.FromArgb(35, 35, 50);
			lblNetHeader.Location = new Point(11, 86);
			lblNetHeader.Name = "lblNetHeader";
			lblNetHeader.Size = new Size(276, 32);
			lblNetHeader.TabIndex = 2;
			lblNetHeader.Text = "Redes WiFi disponibles";
			// 
			// pnlTop
			// 
			pnlTop.BackColor = Color.FromArgb(25, 90, 185);
			pnlTop.Controls.Add(lblCurrentConn);
			pnlTop.Controls.Add(btnDisconnect);
			pnlTop.Dock = DockStyle.Top;
			pnlTop.Location = new Point(0, 0);
			pnlTop.Name = "pnlTop";
			pnlTop.Size = new Size(1056, 68);
			pnlTop.TabIndex = 1;
			// 
			// pnlSeparator
			// 
			pnlSeparator.BackColor = Color.FromArgb(200, 210, 230);
			pnlSeparator.Dock = DockStyle.Top;
			pnlSeparator.Location = new Point(0, 68);
			pnlSeparator.Name = "pnlSeparator";
			pnlSeparator.Size = new Size(1056, 2);
			pnlSeparator.TabIndex = 0;
			// 
			// WifiForm
			// 
			AutoScaleDimensions = new SizeF(12F, 30F);
			AutoScaleMode = AutoScaleMode.Font;
			BackColor = Color.FromArgb(243, 246, 252);
			ClientSize = new Size(1056, 627);
			Controls.Add(pnlSeparator);
			Controls.Add(pnlTop);
			Controls.Add(lblNetHeader);
			Controls.Add(btnRefresh);
			Controls.Add(lvNetworks);
			Controls.Add(pnlConnect);
			Controls.Add(lblStatus);
			Font = new Font("Segoe UI", 9F);
			FormBorderStyle = FormBorderStyle.FixedSingle;
			MaximizeBox = false;
			Name = "WifiForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "WifiPicker";
			pnlConnect.ResumeLayout(false);
			pnlConnect.PerformLayout();
			pnlTop.ResumeLayout(false);
			pnlTop.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		private ListView  lvNetworks;
        private Label     lblCurrentConn;
        private Button    btnDisconnect;
        private Button    btnRefresh;
        private Panel     pnlConnect;
        private Label     lblPanelTitle;
        private Label     lblSSIDTitle;
        private Label     lblSelectedSSID;
        private Label     lblSecTitle;
        private Label     lblSelectedSec;
        private Label     lblPassword;
        private TextBox   txtPassword;
        private CheckBox  chkShowPass;
        private Label     lblConnectHint;
        private Button    btnConnect;
        private Label     lblStatus;
        private Label     lblNetHeader;
        private Panel     pnlTop;
        private Panel     pnlSeparator;
		private Panel sep;
		private Button OlvidartBtn;
	}
}
