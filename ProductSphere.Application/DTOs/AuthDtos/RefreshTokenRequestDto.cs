using System.ComponentModel.DataAnnotations;

namespace ProductSphere.Application.DTOs.AuthDtos
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}