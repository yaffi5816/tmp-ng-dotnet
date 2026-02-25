using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public record UserLoginDTO
    (
        [Required] string UserName,
        [Required] string Password
    );
}
