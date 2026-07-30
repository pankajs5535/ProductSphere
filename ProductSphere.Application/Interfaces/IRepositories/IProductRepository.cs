
using ProductSphere.Domain.Entities;

namespace ProductSphere.Application.Interfaces.IRepositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> ExistsByNameAsync(string productName);
    }
}