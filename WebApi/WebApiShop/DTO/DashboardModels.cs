using System.Text.Json.Serialization;

namespace DTO;

public class DashboardRequest
{
    [JsonPropertyName("schema")]
    public object Schema { get; set; } = new();
    
    [JsonPropertyName("components")]
    public List<string> Components { get; set; } = new();
}

public class GeminiSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";
}
