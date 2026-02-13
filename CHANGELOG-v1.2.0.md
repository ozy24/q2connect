# Q2Connect v1.2.0

## Downloads

- **Q2Connect-v1.2.0-full.zip** (recommended) – Contains the needed .NET 10 runtime files within the executable.
- **Q2Connect-v1.2.0-lite.zip** – Smaller in size but requires a separate .NET 10 runtime install from:
  [.NET 10 Desktop Runtime (Windows x64)](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-10.0.2-windows-x64-installer)

Both releases are otherwise identical.

---

## UI

- **Font size** – Toolbar buttons (Refresh, Connect, Favorite, Settings, Log, About), Address Book “Add” button, and tab headers (Public Servers, Address Book) now use 12pt font to match the server list.
- **Tabs** – Tab styling uses the theme base so dark/light mode is preserved.

## Build & distribution

- **Executable name** – Output executable is now **q2connect.exe** (lowercase). Build scripts and README updated accordingly.
- **Version** – Set to 1.2.0 across the WPF project and About text.

## Fixes

- **Single-file publish** – Config paths use `AppContext.BaseDirectory` instead of `Assembly.Location`, so portable config (favorites, settings, address book) works correctly when published as a single-file app and IL3000 is resolved.
- **About window** – Version display handles a null `Version` without dereference warnings (CS8602).
