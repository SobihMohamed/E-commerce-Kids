using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addedGenderTargetToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TargetGender",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TargetGender",
                table: "Products");
        }
    }
}
