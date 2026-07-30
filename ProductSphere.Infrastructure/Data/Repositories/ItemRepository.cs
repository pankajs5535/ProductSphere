using ProductSphere.Application.Interfaces.IRepositories;
using ProductSphere.Domain.Entities;

namespace ProductSphere.Infrastructure.Data.Repositories
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        public ItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}