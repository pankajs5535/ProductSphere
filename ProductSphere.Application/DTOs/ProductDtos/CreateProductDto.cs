using System.ComponentModel.DataAnnotations;

namespace ProductSphere.Application.DTOs.ProductDtos
{
    public class CreateProductDto
    {
        [Required]
        [MaxLength(255)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;
    }
}   