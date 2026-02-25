using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-code")]
        public async Task<IActionResult> SendCode([FromBody] EmailRequest request)
        {
            try
            {
                await _emailService.SendCodeEmailAsync(request.Email, request.Code, request.FileName);
                return Ok(new { message = "Email sent successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public record EmailRequest(string Email, string Code, string FileName);
}
