using E_commerce.Domain.Models.CustomerInteraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_commerce.Persistence.Configurations
{
    public class CartItemConfig : IEntityTypeConfiguration<CartItemEntity>
    {
        public void Configure(EntityTypeBuilder<CartItemEntity> builder)
        {
            builder.ToTable("CartItems");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Quantity).IsRequired();

            builder.HasOne(c => c.ShoppingCart)
                .WithMany(sc => sc.CartItems)
                .HasForeignKey(c => c.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.ProductVariant)
                .WithMany()
                .HasForeignKey(c => c.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);

            // ==========================================================
            // 🌟 التعديل الجديد: مسح العنصر من السلة لو الديزاين اتلغى
            // ==========================================================
            builder.HasOne(c => c.Design)
                .WithMany()
                .HasForeignKey(c => c.DesignId)
                .OnDelete(DeleteBehavior.Cascade); // 👈 مسح من السلة النشطة
        }
    }
}