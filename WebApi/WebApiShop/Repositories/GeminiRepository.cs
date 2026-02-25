using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using DTO;

namespace Repositories;

public interface IGeminiRepository
{
    Task<string> GenerateDashboardAsync(string prompt);
}

public class GeminiRepository(HttpClient httpClient, IOptions<GeminiSettings> settings) : IGeminiRepository
{
    private readonly GeminiSettings _settings = settings.Value;

    public async Task<string> GenerateDashboardAsync(string prompt)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
            {
                throw new InvalidOperationException("Gemini API Key is not configured. Please check appsettings.json");
            }

            var request = new GeminiRequest
            {
                Contents = new List<Content>
                {
                    new Content
                    {
                        Parts = new List<Part>
                        {
                            new Part { Text = prompt }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"{_settings.BaseUrl}?key={_settings.ApiKey}";
            var response = await httpClient.PostAsync(url, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Gemini API request failed with status {response.StatusCode}. Response: {errorContent}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseJson);

            var htmlCode = geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text ?? string.Empty;
            
            if (string.IsNullOrWhiteSpace(htmlCode))
            {
                throw new InvalidOperationException("Gemini API returned empty response");
            }
            
            return CleanMarkdown(htmlCode);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to generate dashboard: {ex.Message}", ex);
        }
    }

    private static string CleanMarkdown(string text)
    {
        text = Regex.Replace(text, @"```html\s*", string.Empty);
        text = Regex.Replace(text, @"```\s*$", string.Empty);
        return text.Trim();
    }
}
