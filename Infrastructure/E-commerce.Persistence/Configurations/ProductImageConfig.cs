using E_commerce.Domain.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Persistence.Configurations
{
    public class ProductImageConfig : IEntityTypeConfiguration<ProductImageEntity>
    {
        public void Configure(EntityTypeBuilder<ProductImageEntity> builder)
        {
            builder.ToTable("ProductImages");
            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            // العلاقات اتعملت في ProductConfig
            // builder.HasOne(pi => pi.Product)
            //        .WithMany(p => p.Images)
            //        .HasForeignKey(pi => pi.ProductId)
            //        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
