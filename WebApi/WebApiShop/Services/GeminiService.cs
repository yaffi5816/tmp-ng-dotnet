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
}
