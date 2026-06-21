using E_commerce.Domain.Models.Shipping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace E_commerce.Persistence.Configurations
{
    public class ShippingRatesConfig : IEntityTypeConfiguration<ShippingRates>
    {
        public void Configure(EntityTypeBuilder<ShippingRates> builder)
        {
            builder.ToTable("ShippingRates");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.CityName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(s => s.Price)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            DateTime staticDate = new DateTime(2026, 6, 21, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                // القاهرة والجيزة 
                new ShippingRates { Id = 1, CityName = "Cairo (القاهرة)", Price = 80m, CreatedAt = staticDate },
                new ShippingRates { Id = 2, CityName = "Giza (الجيزة)", Price = 80m, CreatedAt = staticDate },

                // الوجه البحري ومدن القناة 
                new ShippingRates { Id = 3, CityName = "Alexandria (الإسكندرية)", Price = 115m, CreatedAt = staticDate },
                new ShippingRates { Id = 4, CityName = "Qalyubia (القليوبية)", Price = 115m, CreatedAt = staticDate },
                new ShippingRates { Id = 5, CityName = "Port Said (بورسعيد)", Price = 115m, CreatedAt = staticDate },
                new ShippingRates { Id = 6, CityName = "Suez (السويس)", Price = 115m, CreatedAt = staticDate },
                new ShippingRates { Id = 7, CityName = "Gharbia (الغربية)", Price = 115m, CreatedAt = staticDate },
                new ShippingRates { Id = 8, CityName = "Dakahlia (الدقهلية)", Price = 115m, CreatedAt = staticDate },
                new ShippingRates { Id = 9, CityName = "Ismailia (الإسماعيلية)", Price = 115m, CreatedAt = staticDate },
                new ShippingRates { Id = 10, CityName = "Sharqia (الشرقية)", Price = 115m, CreatedAt = staticDate },
                new ShippingRates { Id = 11, CityName = "Damietta (دمياط)", Price = 115m, CreatedAt = staticDate },
                new ShippingRates { Id = 12, CityName = "Beheira (البحيرة)", Price = 115m, CreatedAt = staticDate },
                new ShippingRates { Id = 13, CityName = "Kafr El Sheikh (كفر الشيخ)", Price = 115m, CreatedAt = staticDate },
                new ShippingRates { Id = 14, CityName = "Monufia (المنوفية)", Price = 115m, CreatedAt = staticDate },

                // من الفيوم إلى المنيا 
                new ShippingRates { Id = 15, CityName = "Faiyum (الفيوم)", Price = 125m, CreatedAt = staticDate },
                new ShippingRates { Id = 16, CityName = "Beni Suef (بني سويف)", Price = 125m, CreatedAt = staticDate },
                new ShippingRates { Id = 17, CityName = "Minya (المنيا)", Price = 125m, CreatedAt = staticDate },

                // من أسيوط إلى الأقصر 
                new ShippingRates { Id = 18, CityName = "Asyut (أسيوط)", Price = 140m, CreatedAt = staticDate },
                new ShippingRates { Id = 19, CityName = "Sohag (سوهاج)", Price = 140m, CreatedAt = staticDate },
                new ShippingRates { Id = 20, CityName = "Qena (قنا)", Price = 140m, CreatedAt = staticDate },
                new ShippingRates { Id = 21, CityName = "Luxor (الأقصر)", Price = 140m, CreatedAt = staticDate },

                // جنوب الصعيد والمحافظات الحدودية والبحر الأحمر 
                new ShippingRates { Id = 22, CityName = "Aswan (أسوان)", Price = 155m, CreatedAt = staticDate },
                new ShippingRates { Id = 23, CityName = "Red Sea (البحر الأحمر)", Price = 155m, CreatedAt = staticDate },
                new ShippingRates { Id = 24, CityName = "New Valley (الوادي الجديد)", Price = 155m, CreatedAt = staticDate },
                new ShippingRates { Id = 25, CityName = "Matrouh (مطروح)", Price = 155m, CreatedAt = staticDate },
                new ShippingRates { Id = 26, CityName = "North Sinai (شمال سيناء)", Price = 155m, CreatedAt = staticDate },
                new ShippingRates { Id = 27, CityName = "South Sinai (جنوب سيناء)", Price = 155m, CreatedAt = staticDate }
            );
        }
    }
}