using E_commerce.Domain.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce.Persistence.Configurations
{
    public class OrderItemsConfig : IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {
            builder.ToTable("OrderItems");
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(oi => oi.ProductPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.CustomizationPrice)
                   .HasColumnType("decimal(18,2)");

            builder.Property(oi => oi.Quantity)
                .IsRequired();

            builder.HasOne(oi => oi.ProductVariant)
                .WithMany()
                .HasForeignKey(oi => oi.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==========================================================
            // 🌟 التعديل الجديد: فك ارتباط الديزاين أوتوماتيك في الفواتير
            // ==========================================================
            builder.HasOne(oi => oi.Design)
                .WithMany()
                .HasForeignKey(oi => oi.DesignId)
                .OnDelete(DeleteBehavior.SetNull); // 👈 السر كله هنا
        }
    }
}