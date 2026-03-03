using E_commerce.Domain.Models.CustomerInteraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Persistence.Configurations
{
    public class ProductReviewConfig : IEntityTypeConfiguration<ProductReviewEntity>
    {
        public void Configure(EntityTypeBuilder<ProductReviewEntity> builder)
        {
            builder.ToTable("ProductReviews");
            builder.HasKey(pr => pr.Id);

            builder.Property(pr => pr.Rating)
                .IsRequired();

            // Pro Move: 1 - 5 
            builder.ToTable(t => t.HasCheckConstraint("CK_ProductReview_Rating", "Rating >= 1 AND Rating <= 5"));

            builder.Property(pr => pr.Comment)
                .HasMaxLength(1000);
            builder.Property(pr => pr.IsApproved)
                .HasDefaultValue(false);

            // User is in ApplicationUserConfig

            // if product is deleted, delete reviews
            builder.HasOne(pr => pr.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(pr => pr.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // if order is deleted, restrict deletion of reviews (to preserve review history)
            builder.HasOne(pr => pr.Order)
                .WithMany()
                .HasForeignKey(pr => pr.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
