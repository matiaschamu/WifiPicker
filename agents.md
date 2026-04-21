# Contexto del proyecto — WifiPicker

## Descripción
Aplicación de escritorio Windows (WinForms / .NET 10) que permite al usuario explorar redes WiFi cercanas, conectarse, desconectarse y olvidar redes guardadas, todo desde una UI simple en español.

## Stack
- **Framework:** .NET 10 (`net10.0-windows`)
- **UI:** Windows Forms (`UseWindowsForms = true`)
- **Lenguaje:** C# con nullable habilitado e implicit usings
- **Solución:** `WifiPicker.slnx` (formato moderno)
- **Proyecto:** `WifiPicker/WifiPicker.csproj`
- **Dependencias externas:** ninguna (cero paquetes NuGet)

## Estructura de archivos
```
WifiPicker/                     ← carpeta raíz (Desktop)
├── WifiPicker.slnx
└── WifiPicker/
    ├── WifiPicker.csproj
    ├── Program.cs            — entry point, STAThread, ApplicationConfiguration.Initialize()
    ├── WifiForm.cs           — lógica principal
    ├── WifiForm.Designer.cs  — layout generado por el Designer de VS
    ├── WifiForm.resx         — recursos del formulario
    ├── app.manifest          — manifiesto de aplicación Windows
    └── wifi.ico              — ícono embebido en el ensamblado
```

## Modelo de datos
```csharp
enum WifiAuth { Open, WPA, WPA2, WPA3, Enterprise }

class WifiNetwork {
    string   SSID        // nombre de la red
    WifiAuth Auth        // tipo de autenticación
    int      Signal      // porcentaje de señal (0–100)
    bool     HasProfile  // tiene perfil guardado en Windows
    bool     IsConnected // es la red actualmente conectada
}
```

## Lógica central (`WifiForm.cs`)
- **`RefreshAsync()`** — llama a `ScanAsync()`, `GetProfilesAsync()` y `GetCurrentSSIDAsync()` en paralelo lógico, luego actualiza la lista.
- **`ScanAsync()`** — ejecuta `netsh wlan show networks mode=bssid`, parsea bloques por SSID, mergeando duplicados conservando la señal máxima.
- **`GetProfilesAsync()`** — ejecuta `netsh wlan show profiles`, soporta salida en español e inglés.
- **`ConnectAsync()`** — si la red no tiene perfil, crea uno vía `CreateProfileAsync()` antes de conectar. Bloquea redes Enterprise con mensaje informativo.
- **`DisconnectAsync()`** — `netsh wlan disconnect` + refresh.
- **`OlvidarRedAsync()`** — desconecta si es la red activa, luego `netsh wlan delete profile`.
- **`CreateProfileAsync()`** — genera XML de perfil WPA/WPA2/WPA3/Open y lo pasa a `netsh wlan add profile`. El archivo temporal se borra siempre en el `finally`.
- **`RunAsync()`** — `static`, wrapper de `cmd.exe /c chcp 65001 & <args>` con `ReadToEndAsync()` + `WaitForExitAsync()` para async real sin `Task.Run`.
- **Auto-refresh:** `System.Windows.Forms.Timer` cada 20 segundos, con `Dispose()` al cerrar.

## Regex precompilados (`[GeneratedRegex]`)
| Método generado | Uso |
|---|---|
| `ProfilesRegex()` | Parsea perfiles en `GetProfilesAsync` |
| `CurrentSsidRegex()` | Detecta SSID activo en `GetCurrentSSIDAsync` |
| `SsidBlockRegex()` | Divide bloques en `ScanAsync` |
| `SignalRegex()` | Extrae porcentaje de señal en `ScanAsync` |

## UI
- `lvNetworks` — `ListView` de vista detalle con columnas: Red (SSID), Señal, Seguridad, Guardada.
- Red conectada: fondo verde claro + negrita + prefijo `✓`.
- Redes guardadas: texto azul.
- Panel lateral `pnlConnect` — aparece al seleccionar una red; muestra campo de contraseña solo si la red no es Open ni tiene perfil guardado.
- `btnRefresh`, `btnConnect`, `btnDisconnect`, `OlvidartBtn`, `chkShowPass`, `txtPassword`.
- `lblCurrentConn` — muestra la red activa en la barra superior.
- `lblStatus` — barra de estado inferior con conteo de redes y timestamp del último refresh.

## Convenciones del proyecto
- UI y mensajes completamente en español.
- Soporte bilingüe (es/en) en el parseo de salida de `netsh` para que funcione en Windows en cualquier idioma.
- Columnas del `ListView` configuradas en código (`SetupListView`) para que el Designer de VS no las sobreescriba.
- Sanitización XML mediante `Xml()` (escape de `&`, `<`, `>`, `"`) antes de escribir perfiles.
- Escape de comillas dobles en argumentos de `cmd.exe` mediante `EscCmd()`.
- El ícono se carga desde el recurso embebido (`GetManifestResourceStream`).
- Namespace: `WifiPicker` (sin guiones ni espacios).
