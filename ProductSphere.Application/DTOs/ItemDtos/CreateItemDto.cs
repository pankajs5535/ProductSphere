using System.ComponentModel.DataAnnotations;

namespace ProductSphere.Application.DTOs.ItemDtos
{
    public class CreateItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}