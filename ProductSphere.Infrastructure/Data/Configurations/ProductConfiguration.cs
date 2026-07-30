using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductSphere.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProductName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(x => x.CreatedBy)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.CreatedOn)
                   .IsRequired();

            builder.Property(x => x.ModifiedBy)
                   .HasMaxLength(100);

            builder.Property(x => x.ModifiedOn);

            builder.HasMany(x => x.Items)
                   .WithOne(x => x.Product)
                   .HasForeignKey(x => x.ProductId);

        }
    }
}
