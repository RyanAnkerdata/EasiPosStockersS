using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasiPosStockers.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Product_23121210271251 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppProductCostCentre_AppProducts_ProductId",
                table: "AppProductCostCentre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_AppProductCostCentre_AppProducts_ProductId",
                table: "AppProductCostCentre",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id");
        }
    }
}
