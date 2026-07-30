using ProductSphere.Application.DTOs.ProductDtos;

namespace ProductSphere.Application.Interfaces.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();

        Task<ProductDto?> GetByIdAsync(int id);

        Task CreateAsync(CreateProductDto createProductDto);

        Task UpdateAsync(UpdateProductDto updateProductDto);

        Task DeleteAsync(int id);
    }
}