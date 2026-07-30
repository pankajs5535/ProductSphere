using System.ComponentModel.DataAnnotations;

namespace ProductSphere.Application.DTOs.ItemDtos
{
    public class UpdateItemDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}