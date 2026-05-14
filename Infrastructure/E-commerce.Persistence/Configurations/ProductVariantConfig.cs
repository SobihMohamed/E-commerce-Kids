using E_commerce.Domain.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Persistence.Configurations
{
    public class ProductVariantConfig : IEntityTypeConfiguration<ProductVariantEntity>
    {
        public void Configure(EntityTypeBuilder<ProductVariantEntity> builder)
        {
            builder.ToTable("ProductVariants");
            builder.HasKey(pv => pv.Id);

            builder.Property(pv => pv.StockQuantity)
                   .IsRequired()
                   .HasDefaultValue(0);
            // Ensure that the combination of ProductId, ColorId, and SizeId is unique to prevent duplicate variants
            builder.HasIndex(pv => new { pv.ProductId, pv.ColorId, pv.SizeId })
                   .IsUnique()
                   .HasDatabaseName("IX_ProductVariant_Unique_Product_Color_Size");

            builder.HasOne(pv => pv.Color)
                   .WithMany()
                   .HasForeignKey(pv => pv.ColorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(pv => pv.Size)
               .WithMany()
               .HasForeignKey(pv => pv.SizeId)
               .OnDelete(DeleteBehavior.Restrict);
            //created in product configuration
            //builder.HasOne(pv => pv.Product)
            //       .WithMany(p => p.Variants)
            //       .HasForeignKey(pv => pv.ProductId)
            //       .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
