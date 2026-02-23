@echo off
echo ========================================
echo Starting DashGen Application
echo ========================================
echo.

echo [1/3] Stopping any running servers...
taskkill /F /IM dotnet.exe 2>nul
taskkill /F /IM node.exe 2>nul
timeout /t 2 /nobreak >nul

echo.
echo [2/3] Starting Backend Server...
start "DashGen Backend" cmd /k "cd /d c:\Users\y0533\Desktop\DashGen\WebApi\WebApiShop\WebApiShop && echo Building backend... && dotnet build && echo Starting backend server... && dotnet run --launch-profile http"

echo Waiting for backend to initialize...
timeout /t 10 /nobreak >nul

echo.
echo [3/3] Starting Frontend Server...
start "DashGen Frontend" cmd /k "cd /d c:\Users\y0533\Desktop\DashGen\client && echo Starting Angular dev server... && npm start"

echo.
echo ========================================
echo Application is starting!
echo ========================================
echo.
echo Backend will be available at: http://localhost:5034
echo Frontend will be available at: http://localhost:4200
echo.
echo Press any key to close this window...
pause >nul
