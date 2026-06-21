using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_commerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDynamicShippingRates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShippingRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingRates", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ShippingRates",
                columns: new[] { "Id", "CityName", "CreatedAt", "CreatedBy", "IsDeleted", "LastModifiedBy", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Cairo (القاهرة)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 80m, null },
                    { 2, "Giza (الجيزة)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 80m, null },
                    { 3, "Alexandria (الإسكندرية)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 115m, null },
                    { 4, "Qalyubia (القليوبية)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 115m, null },
                    { 5, "Port Said (بورسعيد)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 115m, null },
                    { 6, "Suez (السويس)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 115m, null },
                    { 7, "Gharbia (الغربية)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 115m, null },
                    { 8, "Dakahlia (الدقهلية)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 115m, null },
                    { 9, "Ismailia (الإسماعيلية)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 115m, null },
                    { 10, "Sharqia (الشرقية)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 115m, null },
                    { 11, "Damietta (دمياط)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 115m, null },
                    { 12, "Beheira (البحيرة)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 115m, null },
                    { 13, "Kafr El Sheikh (كفر الشيخ)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 115m, null },
                    { 14, "Monufia (المنوفية)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 115m, null },
                    { 15, "Faiyum (الفيوم)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 125m, null },
                    { 16, "Beni Suef (بني سويف)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 125m, null },
                    { 17, "Minya (المنيا)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 125m, null },
                    { 18, "Asyut (أسيوط)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 140m, null },
                    { 19, "Sohag (سوهاج)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 140m, null },
                    { 20, "Qena (قنا)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 140m, null },
                    { 21, "Luxor (الأقصر)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 140m, null },
                    { 22, "Aswan (أسوان)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 155m, null },
                    { 23, "Red Sea (البحر الأحمر)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 155m, null },
                    { 24, "New Valley (الوادي الجديد)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 155m, null },
                    { 25, "Matrouh (مطروح)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 155m, null },
                    { 26, "North Sinai (شمال سيناء)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 155m, null },
                    { 27, "South Sinai (جنوب سيناء)", new DateTime(2026, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, false, null, 155m, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShippingRates");
        }
    }
}
