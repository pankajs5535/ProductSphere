using ProductSphere.Application.DTOs.ItemDtos;

namespace ProductSphere.Application.Interfaces.IServices
{
    public interface IItemService
    {
        Task<IEnumerable<ItemDto>> GetAllAsync();
        Task<ItemDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateItemDto dto);
        Task UpdateAsync(UpdateItemDto dto);
        Task DeleteAsync(int id);
    }
}