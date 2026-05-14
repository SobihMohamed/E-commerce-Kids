using E_commerce.Domain.Models.Address;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Persistence.Configurations
{
    public class AddressConfig : IEntityTypeConfiguration<AddressEntity>
    {
        public void Configure(EntityTypeBuilder<AddressEntity> builder)
        {
            builder.ToTable("Addresses");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.StreetDetails)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(a => a.IsDefault)
                .HasDefaultValue(false);

            // made in ApplicationUserConfig
            // builder.HasOne(a => a.User)
            //        .WithMany(u => u.Addresses)
            //        .HasForeignKey(a => a.UserId)
            //        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
