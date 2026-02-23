@echo off
echo Starting DashGen Project...
echo.

echo [1/2] Starting Backend (WebApi)...
start "DashGen Backend" cmd /k "cd /d c:\Users\y0533\Desktop\DashGen\WebApi\WebApiShop\WebApiShop && dotnet run"

timeout /t 5 /nobreak >nul

echo [2/2] Starting Frontend (Angular)...
start "DashGen Frontend" cmd /k "cd /d c:\Users\y0533\Desktop\DashGen\client && npm start"

echo.
echo Both servers are starting...
echo Backend will run on: http://localhost:5034 or http://localhost:5000
echo Frontend will run on: http://localhost:4200
echo.
pause
