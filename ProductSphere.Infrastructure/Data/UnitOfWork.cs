using ProductSphere.Application.Interfaces.IRepositories;
using ProductSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductSphere.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IProductRepository Products { get; }

        public IItemRepository Items { get; }


        public UnitOfWork(ApplicationDbContext context, IProductRepository productRepository, IItemRepository itemRepository)
        {
            _context = context;
            Products = productRepository;
            Items = itemRepository;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
