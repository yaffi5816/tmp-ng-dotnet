using Microsoft.AspNetCore.Mvc;
using Services;
using DTO;

namespace WebApiShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IGeminiService _geminiService;

    public DashboardController(IGeminiService geminiService)
    {
        _geminiService = geminiService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> CreateDashboard([FromBody] DashboardRequest request)
    {
        try
        {
            if (request == null || request.Schema == null || request.Components == null)
            {
                return BadRequest(new { error = "Invalid request. Schema and Components are required." });
            }

            var code = await _geminiService.GenerateDashboardCodeAsync(request);
            return Ok(new { generatedCode = code });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack: {ex.StackTrace}");
            return BadRequest(new { error = ex.Message, details = ex.InnerException?.Message });
        }
    }
}
