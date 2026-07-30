
namespace ProductSphere.Application.Interfaces.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }

        IItemRepository Items { get; }

        Task<int> SaveChangesAsync(); // Save All Changes in a Single Commit
    }
}