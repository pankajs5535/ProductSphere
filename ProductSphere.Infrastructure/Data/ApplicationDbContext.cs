using Microsoft.EntityFrameworkCore;
using ProductSphere.Domain.Entities;


namespace ProductSphere.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<Product> Products { get; set; }

        public DbSet<Item> Items { get; set; }

        // JWT_Auth
        public DbSet<User> Users { get; set; }

        // JWT_Auth
        public DbSet<Role> Roles { get; set; }

        // JWT_Auth
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

    }
}
