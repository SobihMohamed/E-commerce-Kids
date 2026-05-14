using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomizationToShoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DesignId",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_DesignId",
                table: "CartItems",
                column: "DesignId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Designs_DesignId",
                table: "CartItems",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Designs_DesignId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_DesignId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "DesignId",
                table: "CartItems");
        }
    }
}
