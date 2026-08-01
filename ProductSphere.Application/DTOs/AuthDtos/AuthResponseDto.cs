using System.ComponentModel.DataAnnotations;

namespace ProductSphere.Application.DTOs.AuthDtos
{
    public class AuthResponseDto
    {
        [Required]
        public string AccessToken { get; set; } = string.Empty;

        [Required]
        public string RefreshToken { get; set; } = string.Empty;

        [Required]
        public DateTime AccessTokenExpiresAtUtc { get; set; }

        [Required]
        public DateTime RefreshTokenExpiresAtUtc { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = string.Empty;
    }
}