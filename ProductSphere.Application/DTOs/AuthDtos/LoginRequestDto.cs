using System.ComponentModel.DataAnnotations;

namespace ProductSphere.Application.DTOs.AuthDtos
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;
    }
}