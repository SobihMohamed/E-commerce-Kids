using E_commerce.Domain.Models.Lookup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Persistence.Configurations
{
    public class SizeConfig : IEntityTypeConfiguration<SizeEntity>
    {
        public void Configure(EntityTypeBuilder<SizeEntity> builder)
        {
            builder.ToTable("Sizes");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(20);  //(S, M, L, XL) 

            // Ensure that the Name is unique across the Sizes table
            builder.HasIndex(s => s.Name)
                   .IsUnique()
                   .HasDatabaseName("IX_Sizes_Unique_Name");
        }
    }
}
