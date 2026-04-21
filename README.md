# WifiPicker

Aplicación de escritorio para Windows que permite explorar, conectarse y gestionar redes WiFi desde una interfaz gráfica simple.

## Características

- **Escaneo de redes** — lista todas las redes WiFi cercanas con nombre (SSID), nivel de señal, tipo de seguridad e indicador de red guardada.
- **Auto-refresco** — la lista se actualiza automáticamente cada 20 segundos.
- **Conexión con contraseña** — soporta redes abiertas, WPA, WPA2 y WPA3. Genera y registra el perfil XML en Windows de forma automática.
- **Redes guardadas** — reconoce perfiles ya almacenados en el sistema y se conecta sin pedir contraseña.
- **Desconexión** — desconecta la interfaz WiFi activa con un clic.
- **Olvidar red** — elimina el perfil guardado y la contraseña asociada.
- **Redes empresariales** — detecta redes WPA2/WPA3-Enterprise e informa que requieren credenciales corporativas.

## Tecnología

- **Plataforma:** .NET 10, Windows Forms (WinForms)
- **Backend WiFi:** `netsh wlan` (Windows built-in CLI)
- **Lenguaje:** C# con nullable habilitado, implicit usings y source generators
- **Regex:** compilados en tiempo de build con `[GeneratedRegex]`
- **I/O async:** `ReadToEndAsync` + `WaitForExitAsync` para procesos, `File.WriteAllTextAsync` para perfiles

## Requisitos

- Windows 10 / 11
- .NET 10 Runtime
- Adaptador WiFi activo

## Uso

1. Abre la aplicación.
2. La lista de redes cercanas se carga automáticamente.
3. Haz clic en una red para ver sus detalles.
4. Ingresa la contraseña (si es necesario) y presiona **Conectar** o haz doble clic en la red.
5. Para olvidar una red guardada, selecciónala y presiona **Olvidar**.
