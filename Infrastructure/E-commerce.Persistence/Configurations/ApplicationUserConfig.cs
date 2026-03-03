using E_commerce.Domain.Models.CustomerInteraction;
using E_commerce.Domain.Models.Notification;
using E_commerce.Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Persistence.Configurations
{
    public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.UserType)
                .HasConversion<string>();

            builder.HasMany(u => u.Addresses)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId)
                // when delete user, delete all addresses
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                // when delete user, do not delete orders
                .OnDelete(DeleteBehavior.NoAction);
        
            builder.HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId)
                // when delete user, delete all notifications
                .OnDelete(DeleteBehavior.Cascade);

            // make <> because is already has relationship with user, and we want to make another relationship with sender
            builder.HasMany<NotificationEntity>()
                .WithOne(n => n.Sender)
                .HasForeignKey(n => n.SenderId)
                // when delete user, do not delete notifications sent by this user
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                // when delete user, do not delete reviews
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.ShoppingCart)
                .WithOne(c => c.User)
                .HasForeignKey<ShoppingCartEntity>(c => c.UserId)
                // when delete user, delete shopping cart
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
