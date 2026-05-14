using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomizationToOrderItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CustomizationPrice",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CustomizedDesignUrl",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DesignId",
                table: "OrderItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesignName",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_DesignId",
                table: "OrderItems",
                column: "DesignId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Designs_DesignId",
                table: "OrderItems",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Designs_DesignId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_DesignId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CustomizationPrice",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CustomizedDesignUrl",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "DesignId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "DesignName",
                table: "OrderItems");
        }
    }
}
