Write-Host "Testing Dashboard API with extended timeout..." -ForegroundColor Yellow
Write-Host ""

$body = @{
    schema = @{
        users = @('id', 'name', 'email')
        orders = @('id', 'userId', 'total')
    }
    components = @('BarChart', 'KPI Card')
} | ConvertTo-Json -Depth 10

Write-Host "Sending request (this may take up to 2 minutes)..." -ForegroundColor Yellow
Write-Host ""

try {
    $response = Invoke-WebRequest -Uri "http://localhost:5034/api/dashboard/generate" `
        -Method Post `
        -Body $body `
        -ContentType "application/json" `
        -TimeoutSec 120 `
        -ErrorAction Stop
    
    Write-Host ""
    Write-Host "SUCCESS!" -ForegroundColor Green
    Write-Host "Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host ""
    
    $result = $response.Content | ConvertFrom-Json
    Write-Host "Generated Code Length: $($result.generatedCode.Length) characters" -ForegroundColor Green
    Write-Host ""
    Write-Host "First 500 characters:" -ForegroundColor Cyan
    Write-Host $result.generatedCode.Substring(0, [Math]::Min(500, $result.generatedCode.Length))
    
} catch {
    Write-Host ""
    Write-Host "ERROR!" -ForegroundColor Red
    
    if ($_.Exception.Response) {
        Write-Host "Status: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Red
        
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $reader.BaseStream.Position = 0
        $reader.DiscardBufferedData()
        $responseBody = $reader.ReadToEnd()
        
        Write-Host ""
        Write-Host "Error Response:" -ForegroundColor Red
        Write-Host $responseBody
    } else {
        Write-Host $_.Exception.Message -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey('NoEcho,IncludeKeyDown')
