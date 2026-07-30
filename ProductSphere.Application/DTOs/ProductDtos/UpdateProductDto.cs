using System.ComponentModel.DataAnnotations;

namespace ProductSphere.Application.DTOs.ProductDtos
{
    public class UpdateProductDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string ProductName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ModifiedBy { get; set; }
    }
}