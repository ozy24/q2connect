@echo off
echo Building Q2Connect for distribution (Self-Contained - LARGE FILE)...
echo.
echo This creates a ~60MB self-contained executable that includes the .NET runtime.
echo No additional dependencies required - works on any Windows 10/11 PC.
echo.
echo For a smaller build, use build-dist-small.bat instead.
echo Note: Smaller version requires .NET 10 runtime to be installed
echo.

REM Create dist directory
if not exist "dist" mkdir dist
if exist "dist\*" (
    echo Cleaning dist directory...
    del /q "dist\*"
    for /d %%d in ("dist\*") do rmdir /s /q "%%d"
)

echo.
echo Building and publishing Q2Connect.Wpf to dist folder...
dotnet publish Q2Connect.Wpf/Q2Connect.Wpf.csproj ^
    --configuration Release ^
    --output "dist" ^
    --self-contained true ^
    --runtime win-x64 ^
    -p:PublishSingleFile=true ^
    -p:IncludeNativeLibrariesForSelfExtract=true ^
    -p:EnableCompressionInSingleFile=true ^
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
    echo   - Q2Connect.Wpf.exe (main executable)
    echo   - README.md
    echo.
    echo The application is ready for distribution.
    echo.
) else (
    echo.
    echo ========================================
    echo Build failed with error code %ERRORLEVEL%
    echo ========================================
    exit /b %ERRORLEVEL%
)

