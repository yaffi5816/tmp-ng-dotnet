namespace Services;

public interface IDashboardService
{
    Task<string> GenerateDashboardFromSchemaAsync(string schema, string? products = null);
    Task<string> GenerateDashboardFromFileAsync(Stream fileStream, string? products = null);
    Task<string> RefineDashboardAsync(string currentCode, string refinementRequest);
}

public class DashboardService(Repositories.IGeminiRepository geminiRepository) : IDashboardService
{
    public async Task<string> GenerateDashboardFromSchemaAsync(string schema, string? products = null)
    {
        var productsInfo = string.IsNullOrEmpty(products) ? "" : $"\n\nSelected Products/Templates:\n{products}\n\nUse these products as the basis for the dashboard design and styling.";
        
        var prompt = $@"Generate a complete, interactive HTML dashboard based on this schema:
{schema}{productsInfo}

Requirements:
- Create a modern, responsive dashboard with charts and tables
- If products are provided, use their design style and templates
- Include inline CSS and JavaScript
- Use Chart.js or similar for visualizations
- Make it production-ready and visually appealing
- Match the styling to the selected products/templates
- Return ONLY the HTML code, no explanations";

        return await geminiRepository.GenerateDashboardAsync(prompt);
    }

    public async Task<string> GenerateDashboardFromFileAsync(Stream fileStream, string? products = null)
    {
        using var reader = new StreamReader(fileStream);
        var schema = await reader.ReadToEndAsync();
        return await GenerateDashboardFromSchemaAsync(schema, products);
    }

    public async Task<string> RefineDashboardAsync(string currentCode, string refinementRequest)
    {
        var prompt = $@"Refine this HTML dashboard code based on the following request:

Current Code:
{currentCode}

Refinement Request:
{refinementRequest}

Requirements:
- Apply the requested changes to the code
- Maintain all existing functionality
- Keep the code production-ready
- Return ONLY the updated HTML code, no explanations";

        return await geminiRepository.GenerateDashboardAsync(prompt);
    }
}
