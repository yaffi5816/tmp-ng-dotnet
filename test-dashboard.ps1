Write-Host "Testing Dashboard API..." -ForegroundColor Yellow
Write-Host ""

$body = @{
    schema = @{
        users = @('id', 'name', 'email')
        orders = @('id', 'userId', 'total')
    }
    components = @('BarChart', 'KPI Card')
} | ConvertTo-Json -Depth 10

Write-Host "Request Body:" -ForegroundColor Cyan
Write-Host $body
Write-Host ""

try {
    Write-Host "Sending request to http://localhost:5034/api/dashboard/generate..." -ForegroundColor Yellow
    
    $response = Invoke-WebRequest -Uri "http://localhost:5034/api/dashboard/generate" `
        -Method Post `
        -Body $body `
        -ContentType "application/json" `
        -ErrorAction Stop
    
    Write-Host ""
    Write-Host "SUCCESS! Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host ""
    
    $result = $response.Content | ConvertFrom-Json
    Write-Host "Generated Code Length: $($result.generatedCode.Length) characters" -ForegroundColor Green
    Write-Host ""
    Write-Host "First 300 characters:" -ForegroundColor Cyan
    Write-Host $result.generatedCode.Substring(0, [Math]::Min(300, $result.generatedCode.Length))
    
} catch {
    Write-Host ""
    Write-Host "ERROR!" -ForegroundColor Red
    Write-Host "Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
    Write-Host ""
    
    # Read the error response body
    $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
    $reader.BaseStream.Position = 0
    $reader.DiscardBufferedData()
    $responseBody = $reader.ReadToEnd()
    
    Write-Host "Error Response:" -ForegroundColor Red
    Write-Host $responseBody
    Write-Host ""
    Write-Host "Exception Message:" -ForegroundColor Red
    Write-Host $_.Exception.Message
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
