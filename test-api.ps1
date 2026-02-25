$body = @{
    schema = @{
        users = @('id', 'name', 'email')
        orders = @('id', 'userId', 'total', 'date')
    }
    components = @('BarChart', 'KPI Card', 'Table')
} | ConvertTo-Json

Write-Host "Sending request to API..." -ForegroundColor Yellow
Write-Host "Body: $body" -ForegroundColor Cyan

try {
    $response = Invoke-RestMethod -Uri "http://localhost:5034/api/dashboard/generate" -Method Post -Body $body -ContentType "application/json"
    Write-Host "`nSuccess!" -ForegroundColor Green
    Write-Host "Generated Code Length: $($response.generatedCode.Length) characters" -ForegroundColor Green
    Write-Host "`nFirst 500 characters:" -ForegroundColor Yellow
    Write-Host $response.generatedCode.Substring(0, [Math]::Min(500, $response.generatedCode.Length))
} catch {
    Write-Host "`nError:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    if ($_.ErrorDetails) {
        Write-Host $_.ErrorDetails.Message -ForegroundColor Red
    }
}
