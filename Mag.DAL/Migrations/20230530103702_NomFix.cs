using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mag.DAL.Migrations
{
    /// <inheritdoc />
    public partial class NomFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Noms_Stocks_StockId",
                table: "Noms");

            migrationBuilder.DropIndex(
                name: "IX_Noms_StockId",
                table: "Noms");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "Noms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "Noms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Noms_StockId",
                table: "Noms",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Noms_Stocks_StockId",
                table: "Noms",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
