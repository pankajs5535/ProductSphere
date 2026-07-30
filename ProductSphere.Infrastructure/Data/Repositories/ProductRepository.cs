using Microsoft.EntityFrameworkCore;
using ProductSphere.Application.Interfaces.IRepositories;
using ProductSphere.Domain.Entities;

namespace ProductSphere.Infrastructure.Data.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByNameAsync(string productName)
        {
            return await _dbSet.AnyAsync(x => x.ProductName == productName);
        }
    }
}