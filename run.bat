@echo off
echo Running Q2Connect...
dotnet run --project Q2Connect.Wpf/Q2Connect.Wpf.csproj
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Application exited with error code %ERRORLEVEL%
    pause
)









