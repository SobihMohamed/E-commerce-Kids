using E_commerce.Domain.Models.Designs;
using E_commerce.Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Persistence.Configurations
{
    public class DesignsConfig : IEntityTypeConfiguration<DesignsEntity>
    {
        public void Configure(EntityTypeBuilder<DesignsEntity> builder)
        {
            builder.ToTable("Designs");
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(d => d.ImageUrl)
                .IsRequired()
                .HasMaxLength(200);
            builder.Property(d => d.DesignGender)
                .HasConversion<string>();
        }
    }
}
