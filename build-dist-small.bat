@echo off
echo Building Q2Connect for distribution (Framework-Dependent - SMALLER)...
echo.
echo NOTE: This version requires .NET 10 runtime to be installed on the target machine.
echo       The executable will be much smaller but requires .NET runtime.
echo       Users can download .NET from: https://dotnet.microsoft.com/download/dotnet/10.0
echo.

REM Create dist directory
if not exist "dist" mkdir dist
if exist "dist\*" (
    echo Cleaning dist directory...
    del /q "dist\*"
    for /d %%d in ("dist\*") do rmdir /s /q "%%d"
)

echo.
echo Building and publishing Q2Connect.Wpf to dist folder (framework-dependent)...
dotnet publish Q2Connect.Wpf/Q2Connect.Wpf.csproj ^
    --configuration Release ^
    --output "dist" ^
    --self-contained false ^
    --runtime win-x64 ^
    -p:PublishSingleFile=false ^
    -p:DebugType=None ^
    -p:DebugSymbols=false

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Copying README to dist...
    copy README.md dist\README.md >nul
    
    echo.
    echo Removing debug symbols (.pdb files)...
    del /q "dist\*.pdb" 2>nul
    
    echo.
    echo ========================================
    echo Build completed successfully!
    echo ========================================
    echo.
    echo Output location: %CD%\dist
    echo.
    echo Distribution files:
    echo   - Q2Connect.Wpf.exe (main executable, ~400KB)
    echo   - Q2Connect.Wpf.dll (main library, ~600KB)
    echo   - Q2Connect.Core.dll (core library, ~50KB)
    echo   - README.md
    echo   - Configuration files
    echo.
    echo IMPORTANT: This version requires .NET 10 runtime to be installed.
    echo            Users can download from: https://dotnet.microsoft.com/download/dotnet/10.0
    echo.
) else (
    echo.
    echo ========================================
    echo Build failed with error code %ERRORLEVEL%
    echo ========================================
    exit /b %ERRORLEVEL%
)

