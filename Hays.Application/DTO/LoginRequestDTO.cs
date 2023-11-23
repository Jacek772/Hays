using System.ComponentModel.DataAnnotations;

namespace Hays.Application.DTO
{
    public class LoginRequestDTO
    {
        [Required]
        public string Login { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;
    }
}
