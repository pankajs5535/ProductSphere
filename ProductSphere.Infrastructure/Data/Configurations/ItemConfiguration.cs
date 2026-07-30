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
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Quantity)
                 .IsRequired();

            builder.HasOne(x => x.Product)
                   .WithMany(x => x.Items)
                   .HasForeignKey(x => x.ProductId);

        }
    }
}
