using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prjWebCsBennyStockWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddItemAndQuantityToTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Transfers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "Transfers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ItemId",
                table: "Transfers",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Items_ItemId",
                table: "Transfers",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Items_ItemId",
                table: "Transfers");

            migrationBuilder.DropIndex(
                name: "IX_Transfers_ItemId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Transfers");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Transfers");
        }
    }
}
