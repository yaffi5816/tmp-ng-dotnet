$apiKey = "AIzaSyDhYGzYRPEldwM0jLq30Td8lkYD8jj4Dp4"

Write-Host "Checking available Gemini models..." -ForegroundColor Yellow
Write-Host ""

try {
    $response = Invoke-RestMethod -Uri "https://generativelanguage.googleapis.com/v1beta/models?key=$apiKey"
    
    Write-Host "Available Models:" -ForegroundColor Green
    Write-Host ""
    
    foreach ($model in $response.models) {
        if ($model.supportedGenerationMethods -contains "generateContent") {
            Write-Host "OK $($model.name)" -ForegroundColor Green
            Write-Host "  Display Name: $($model.displayName)" -ForegroundColor Cyan
            Write-Host ""
        }
    }
} catch {
    Write-Host "Error checking models:" -ForegroundColor Red
    Write-Host $_.Exception.Message
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')
