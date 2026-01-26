@echo off
echo ========================================
echo Building Q2Connect Release Distribution
echo ========================================
echo.

REM Get version from project file using PowerShell
for /f "delims=" %%v in ('powershell -Command "(Select-Xml -Path 'Q2Connect.Wpf\Q2Connect.Wpf.csproj' -XPath '/Project/PropertyGroup/Version').Node.InnerText"') do set VERSION=%%v
if "%VERSION%"=="" set VERSION=1.1.0
echo Version: %VERSION%
echo.

REM Create temp directories
if exist "dist-temp-self-contained" rmdir /s /q "dist-temp-self-contained"
if exist "dist-temp-framework-dependent" rmdir /s /q "dist-temp-framework-dependent"
if not exist "dist" mkdir dist

echo.
echo ========================================
echo Building Self-Contained Version (~62MB)
echo ========================================
echo.

dotnet publish Q2Connect.Wpf/Q2Connect.Wpf.csproj ^
    --configuration Release ^
    --output "dist-temp-self-contained" ^
    --self-contained true ^
    --runtime win-x64 ^
    -p:PublishSingleFile=true ^
    -p:IncludeNativeLibrariesForSelfExtract=true ^
    -p:EnableCompressionInSingleFile=true ^
    -p:DebugType=None ^
    -p:DebugSymbols=false

if %ERRORLEVEL% NEQ 0 (
    echo Build failed!
    exit /b %ERRORLEVEL%
)

REM Copy README to self-contained build
copy README.md dist-temp-self-contained\README.md >nul

REM Remove any PDB files
del /q "dist-temp-self-contained\*.pdb" 2>nul

REM Create zip for self-contained version
echo.
echo Creating zip file for self-contained version...
powershell -Command "Compress-Archive -Path 'dist-temp-self-contained\*' -DestinationPath 'dist\Q2Connect-v%VERSION%-self-contained.zip' -Force"

echo.
echo ========================================
echo Building Framework-Dependent Version (~1MB)
echo ========================================
echo.

dotnet publish Q2Connect.Wpf/Q2Connect.Wpf.csproj ^
    --configuration Release ^
    --output "dist-temp-framework-dependent" ^
    --self-contained false ^
    --runtime win-x64 ^
    -p:PublishSingleFile=false ^
    -p:DebugType=None ^
    -p:DebugSymbols=false

if %ERRORLEVEL% NEQ 0 (
    echo Build failed!
    exit /b %ERRORLEVEL%
)

REM Copy README to framework-dependent build
copy README.md dist-temp-framework-dependent\README.md >nul

REM Remove any PDB files
del /q "dist-temp-framework-dependent\*.pdb" 2>nul

REM Create zip for framework-dependent version
echo.
echo Creating zip file for framework-dependent version...
powershell -Command "Compress-Archive -Path 'dist-temp-framework-dependent\*' -DestinationPath 'dist\Q2Connect-v%VERSION%-framework-dependent.zip' -Force"

REM Clean up temp directories
echo.
echo Cleaning up temporary files...
rmdir /s /q "dist-temp-self-contained"
rmdir /s /q "dist-temp-framework-dependent"

REM Clean up any loose files in dist (keep only zip files)
del /q "dist\*.exe" 2>nul
del /q "dist\*.dll" 2>nul
del /q "dist\*.json" 2>nul
del /q "dist\*.md" 2>nul

echo.
echo ========================================
echo Build Complete!
echo ========================================
echo.
echo Created files in dist folder:
echo   - Q2Connect-v%VERSION%-self-contained.zip (~62MB, no dependencies)
echo   - Q2Connect-v%VERSION%-framework-dependent.zip (~1MB, requires .NET 10)
echo.
echo Self-contained: Works on any Windows 10/11 PC
echo Framework-dependent: Requires .NET 10 runtime (download from dotnet.microsoft.com)
echo.

