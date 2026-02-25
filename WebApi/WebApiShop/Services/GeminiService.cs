using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using DTO;

namespace Services;

public interface IGeminiService
{
    Task<string> GenerateDashboardCodeAsync(DashboardRequest request);
}

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly GeminiSettings _settings;

    public GeminiService(HttpClient httpClient, IOptions<GeminiSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromMinutes(2); // הגדלת timeout ל-2 דקות
        _settings = settings.Value;
    }

    public async Task<string> GenerateDashboardCodeAsync(DashboardRequest request)
    {
        // אם המשתמש ביקש לדלג על Gemini, החזר HTML דמה
        if (request.SkipGemini)
        {
            return GenerateMockDashboard(request);
        }

        if (string.IsNullOrWhiteSpace(_settings.ApiKey))
        {
            throw new Exception("Gemini API Key is not configured");
        }

        var prompt = $@"
Act as a Senior Full-Stack Developer.
Generate a complete, standalone HTML dashboard with embedded JavaScript and CSS.
Database Schema provided: {JsonSerializer.Serialize(request.Schema)}
Components to include: {string.Join(", ", request.Components)}

Rules:
1. Return a COMPLETE HTML file with <!DOCTYPE html>, <html>, <head>, and <body> tags.
2. Include Chart.js library from CDN for charts (NOT Recharts - use Chart.js instead).
3. Include Tailwind CSS from CDN.
4. All JavaScript must be embedded in <script> tags.
5. All CSS must be embedded in <style> tags or use Tailwind classes.
6. Data should be hardcoded as sample data in JavaScript.
7. Return ONLY the raw HTML code. No markdown, no '```html' tags, no explanations.
8. The dashboard must be fully functional and display immediately when opened.
9. Use modern, professional design with charts, cards, and tables based on the schema.";

        var payload = new
        {
            contents = new[] { new { parts = new[] { new { text = prompt } } } }
        };

        var url = $"{_settings.BaseUrl}?key={_settings.ApiKey}";
        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Gemini API failed: {response.StatusCode} - {errorContent}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(jsonResponse);

        return doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text").GetString() ?? string.Empty;
    }

    private string GenerateMockDashboard(DashboardRequest request)
    {
        var html = @"<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Dashboard Demo</title>
    <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>
    <script src='https://cdn.tailwindcss.com'></script>
</head>
<body class='bg-gray-100 p-8'>
    <div class='max-w-7xl mx-auto'>
        <h1 class='text-3xl font-bold mb-8 text-gray-800'>Dashboard Demo</h1>
        <div class='grid grid-cols-1 md:grid-cols-3 gap-6 mb-8'>
            <div class='bg-white p-6 rounded-lg shadow'>
                <h3 class='text-gray-500 text-sm'>Total Users</h3>
                <p class='text-3xl font-bold text-blue-600'>1,234</p>
            </div>
            <div class='bg-white p-6 rounded-lg shadow'>
                <h3 class='text-gray-500 text-sm'>Orders</h3>
                <p class='text-3xl font-bold text-green-600'>567</p>
            </div>
            <div class='bg-white p-6 rounded-lg shadow'>
                <h3 class='text-gray-500 text-sm'>Revenue</h3>
                <p class='text-3xl font-bold text-purple-600'>$89,000</p>
            </div>
        </div>
        <div class='bg-white p-6 rounded-lg shadow mb-8'>
            <h2 class='text-xl font-bold mb-4'>Monthly Sales</h2>
            <canvas id='myChart'></canvas>
        </div>
        <div class='bg-white p-6 rounded-lg shadow'>
            <h2 class='text-xl font-bold mb-4'>Recent Data</h2>
            <table class='w-full'>
                <thead>
                    <tr class='border-b'>
                        <th class='text-left p-2'>ID</th>
                        <th class='text-left p-2'>Name</th>
                        <th class='text-left p-2'>Status</th>
                        <th class='text-left p-2'>Amount</th>
                    </tr>
                </thead>
                <tbody>
                    <tr class='border-b'>
                        <td class='p-2'>001</td>
                        <td class='p-2'>John Doe</td>
                        <td class='p-2'><span class='bg-green-100 text-green-800 px-2 py-1 rounded'>Active</span></td>
                        <td class='p-2'>$1,200</td>
                    </tr>
                    <tr class='border-b'>
                        <td class='p-2'>002</td>
                        <td class='p-2'>Jane Smith</td>
                        <td class='p-2'><span class='bg-green-100 text-green-800 px-2 py-1 rounded'>Active</span></td>
                        <td class='p-2'>$2,400</td>
                    </tr>
                    <tr class='border-b'>
                        <td class='p-2'>003</td>
                        <td class='p-2'>Bob Johnson</td>
                        <td class='p-2'><span class='bg-yellow-100 text-yellow-800 px-2 py-1 rounded'>Pending</span></td>
                        <td class='p-2'>$800</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <script>
        const ctx = document.getElementById('myChart');
        new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
                datasets: [{
                    label: 'Sales',
                    data: [12, 19, 3, 5, 2, 3],
                    backgroundColor: 'rgba(54, 162, 235, 0.5)',
                    borderColor: 'rgba(54, 162, 235, 1)',
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    </script>
</body>
</html>";
        return html;
    }
}
