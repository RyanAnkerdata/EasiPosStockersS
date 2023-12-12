using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasiPosStockers.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Product_23121210111923 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppProductCostCentre",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CostCentreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppProductCostCentre", x => new { x.ProductId, x.CostCentreId });
                    table.ForeignKey(
                        name: "FK_AppProductCostCentre_AppCostCentres_CostCentreId",
                        column: x => x.CostCentreId,
                        principalTable: "AppCostCentres",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppProductCostCentre_AppProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "AppProducts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppProductCostCentre_CostCentreId",
                table: "AppProductCostCentre",
                column: "CostCentreId");

            migrationBuilder.CreateIndex(
                name: "IX_AppProductCostCentre_ProductId_CostCentreId",
                table: "AppProductCostCentre",
                columns: new[] { "ProductId", "CostCentreId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppProductCostCentre");
        }
    }
}
