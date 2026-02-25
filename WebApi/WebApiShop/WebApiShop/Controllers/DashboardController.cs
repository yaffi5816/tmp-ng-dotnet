using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApiShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(IDashboardService dashboardService) : ControllerBase
{
    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromForm] string? schema, [FromForm] IFormFile? schemaFile, [FromForm] string? products)
    {
        try
        {
            string htmlResult;

            if (schemaFile != null && schemaFile.Length > 0)
            {
                using var stream = schemaFile.OpenReadStream();
                htmlResult = await dashboardService.GenerateDashboardFromFileAsync(stream, products);
            }
            else if (!string.IsNullOrWhiteSpace(schema))
            {
                htmlResult = await dashboardService.GenerateDashboardFromSchemaAsync(schema, products);
            }
            else
            {
                return BadRequest("Please provide either schema text or a schema file.");
            }

            return Ok(new { html = htmlResult });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Dashboard generation error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return StatusCode(500, new { error = ex.Message, details = ex.InnerException?.Message });
        }
    }

    [HttpPost("refine")]
    public async Task<IActionResult> Refine([FromForm] string currentCode, [FromForm] string refinementRequest)
    {
        try
        {
            var htmlResult = await dashboardService.RefineDashboardAsync(currentCode, refinementRequest);
            return Ok(new { html = htmlResult });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Dashboard refinement error: {ex.Message}");
            return StatusCode(500, new { error = ex.Message, details = ex.InnerException?.Message });
        }
    }
}
