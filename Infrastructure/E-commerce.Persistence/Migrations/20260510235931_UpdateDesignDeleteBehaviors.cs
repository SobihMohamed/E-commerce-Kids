using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_commerce.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDesignDeleteBehaviors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Designs_DesignId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Designs_DesignId",
                table: "OrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Designs_DesignId",
                table: "CartItems",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Designs_DesignId",
                table: "OrderItems",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Designs_DesignId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Designs_DesignId",
                table: "OrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Designs_DesignId",
                table: "CartItems",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Designs_DesignId",
                table: "OrderItems",
                column: "DesignId",
                principalTable: "Designs",
                principalColumn: "Id");
        }
    }
}
