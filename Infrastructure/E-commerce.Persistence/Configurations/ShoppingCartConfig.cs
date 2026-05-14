using E_commerce.Domain.Models.CustomerInteraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Persistence.Configurations
{
    public class ShoppingCartConfig : IEntityTypeConfiguration<ShoppingCartEntity>
    {
        public void Configure(EntityTypeBuilder<ShoppingCartEntity> builder)
        {
            builder.ToTable("ShoppingCarts");
            builder.HasKey(s => s.Id);

            // created relationships in the other entities, so no need to configure them here,
            // but if you want to configure them here, you can uncomment the following code

            //builder.HasOne(s => s.User)
            //       .WithOne()
            //       .HasForeignKey<ShoppingCartEntity>(s => s.UserId)
            //       .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(s => s.CartItems)
            //            .WithOne(i => i.ShoppingCart)
            //            .HasForeignKey(i => i.ShoppingCartId)
            //            .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
