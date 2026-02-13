# Q2Connect

A high-performance, native Windows desktop application to browse Quake II multiplayer servers (original Quake 2, not re-release). Built with .NET 10 and WPF, featuring native system theme support (automatically follows Windows dark/light theme).

**Author:** ozy  
**Repository:** https://github.com/ozy24/q2connect

## Screenshots

![Main Window](img/1.png)

![Server List](img/2.png)

## Features

- ✅ Query Quake II master servers
- ✅ Probe individual game servers with throttled concurrent requests
- ✅ Real-time server list updates (non-blocking UI)
- ✅ Search and filter servers (by name, map, mod, or player names)
- ✅ View players on selected server (sorted by score)
- ✅ Favorites persistence
- ✅ Address Book for manual server entries
  - Add/edit/delete custom server addresses
  - Support for IP addresses, hostnames, FQDNs (with or without ports)
  - Custom labels for easy identification
  - Add servers from server list directly to address book
- ✅ Direct launch Quake 2 executable with server connection
- ✅ Native system theme support (automatically follows Windows dark/light theme via .NET 10 ThemeMode)

---

## For Users

### Prerequisites

- **Windows 10/11** (x64)
- **Quake 2** executable (quake2.exe, q2pro.exe, r1q2.exe, etc.)

### Installation & Running

1. **Download and extract** the application files
2. **Run q2connect.exe** (or use `dotnet run --project Q2Connect.Wpf/Q2Connect.Wpf.csproj` if running from source)

### Getting Started

1. **Launch the application**
2. **Configure settings** (click "Settings" button):
   - **Quake 2 Executable**: Click "Browse..." to select your Quake 2 executable
   - **Master Server**: Configure HTTP or UDP master server settings (defaults work for most users)
   - **Options**: Enable/disable refresh on startup, LAN broadcast discovery
3. **Click "Refresh"** to query the master server and discover game servers
4. **Filter servers** by name, map, mod, or player names using the filter box
5. **View players** by selecting a server - the players list shows all players currently on the server (sorted by score)
6. **Double-click a server** or select it and click "Connect" to launch Quake 2
7. **Toggle favorites** by selecting a server and clicking "Toggle Favorite"
8. **Use the Address Book** tab to manage custom server entries:
   - **Add**: Enter an address (IP, hostname, or FQDN with optional port) and an optional label, then click "Add"
   - **Edit**: Select an entry and click "Edit" to modify the address or label
   - **Delete**: Select an entry and click "Delete" to remove it
   - **Connect**: Double-click an entry or select it and click "Connect" to launch Quake 2
   - **Add from server list**: Select a server from the server list and use "Add to Address Book" (right-click menu or button)

### Keyboard Shortcuts

- **Alt+R** - Refresh server list
- **Alt+C** - Connect to selected server
- **Alt+F** - Add selected server to favorites (when not favorited)
- **Alt+U** - Remove selected server from favorites (when favorited)
- **Alt+S** - Open Settings window
- **Alt+L** - Open Log window
- **Alt+A** - Open About window
- **Enter/Return** - Connect to selected server (when server list has focus)
- **Tab** - Cycle between "Public Servers" and "Address Book" tabs

### Settings

Settings and favorites are stored in different locations depending on the **Portable Mode** setting:

**Portable Mode (Default - Enabled):**
- Settings: `settings.json` in the same directory as the executable
- Favorites: `favorites.json` in the same directory as the executable
- Address Book: `addressbook.json` in the same directory as the executable

**AppData Mode (Portable Mode Disabled):**
- Settings: `%AppData%\Q2Connect\settings.json`
- Favorites: `%AppData%\Q2Connect\favorites.json`
- Address Book: `%AppData%\Q2Connect\addressbook.json`

**Notes:**
- Portable mode is enabled by default, making it easy to keep all application data with the executable
- You can switch between modes in the Settings window
- When switching modes, the old files are preserved (not deleted) in case you want to switch back
- **Log Level**: Controls log verbosity. Default is "Warning" (shows only warnings and errors). Can be configured in Settings → Advanced. Set to "Debug" for detailed troubleshooting logs.
- **Include Player Names in Search**: When enabled (Settings → Advanced), the search filter will also match player names on servers, making it easy to find servers where specific players are playing

### Troubleshooting

- **Quake 2 executable not launching**: Check that the Quake 2 executable path in Settings points to a valid executable (quake2.exe, q2pro.exe, r1q2.exe, etc.)
- **No servers found**: Check your internet connection and firewall settings (UDP port 27900)
- **UI freezes during refresh**: Reduce `MaxConcurrentProbes` in Settings → Advanced

---

## For Developers

### Prerequisites

- **.NET 10 SDK** - Download from [https://dotnet.microsoft.com/download/dotnet/10.0](https://dotnet.microsoft.com/download/dotnet/10.0)
- **Windows 10/11** (x64)
- **Visual Studio Code** with the **C# extension** (ms-dotnettools.csharp) - optional, any IDE with .NET support works

### Building the Project

#### Using Visual Studio Code

1. **Open the project folder** in Visual Studio Code:
   ```bash
   code .
   ```

2. **Install the C# extension** (if not already installed):
   - Press `Ctrl+Shift+X` to open Extensions
   - Search for "C#" by Microsoft
   - Click Install

3. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

4. **Build the solution**:
   ```bash
   dotnet build
   ```

   Or build a specific project:
   ```bash
   dotnet build Q2Connect.Wpf/Q2Connect.Wpf.csproj
   ```

5. **Run the application**:
   ```bash
   dotnet run --project Q2Connect.Wpf/Q2Connect.Wpf.csproj
   ```

#### Using Command Line

Open PowerShell or Command Prompt in the project root and run:

```powershell
# Restore NuGet packages
dotnet restore

# Build the entire solution
dotnet build Q2Connect.sln

# Run the WPF application
dotnet run --project Q2Connect.Wpf/Q2Connect.Wpf.csproj
```

#### Publishing for Release

To create a distributable single-file executable:

```powershell
# Using the provided script
.\publish.bat

# Or manually
dotnet publish Q2Connect.Wpf/Q2Connect.Wpf.csproj --configuration Release --output "publish" --self-contained true --runtime win-x64 -p:PublishSingleFile=true
```

The published executable will be in the `publish` folder. This creates a self-contained, single-file executable that includes the .NET runtime and can be distributed without requiring .NET to be installed on the target machine.

### Project Structure

```
Q2Connect.sln
├── Q2Connect.Core/          # Core library (no WPF dependencies)
│   ├── Models/              # ServerEntry, Settings
│   ├── Networking/          # MasterServerClient, GameServerProbe
│   ├── Protocol/           # PacketHeader, Q2ColorParser, ByteReader
│   └── Services/           # FavoritesService
│
├── Q2Connect.Wpf/          # WPF application
│   ├── ViewModels/         # MainViewModel, ServerRowViewModel
│   ├── Views/             # MainWindow.xaml
│   ├── Converters/        # Q2ColorToBrushConverter
│   └── Services/          # LauncherService
│
└── Q2Connect.Core.Tests/   # Unit tests
    ├── UrlValidatorTests.cs
    ├── PacketHeaderTests.cs
    ├── ByteReaderTests.cs
    └── SettingsTests.cs
```

### Development Notes

- The Core library (`Q2Connect.Core`) has no WPF dependencies, making it portable for future cross-platform migrations
- Server probing is throttled to 75 concurrent requests by default to prevent router packet loss
- UI updates are batched every 150ms to prevent thread saturation
- All networking operations are fully async and non-blocking
- Comprehensive unit tests (61 tests) covering protocol parsing, validation, and core functionality

### Troubleshooting Build Issues

- **Missing .NET 10 SDK**: Ensure you have .NET 10 SDK installed (`dotnet --version` should show 10.x)
- **NuGet restore fails**: Try `dotnet nuget locals all --clear` then `dotnet restore`

---

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
