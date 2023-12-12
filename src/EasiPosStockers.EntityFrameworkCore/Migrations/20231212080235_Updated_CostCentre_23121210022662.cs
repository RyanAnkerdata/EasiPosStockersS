using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasiPosStockers.Migrations
{
    /// <inheritdoc />
    public partial class Updated_CostCentre_23121210022662 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCostCentres_AppProducts_ProductId",
                table: "AppCostCentres");

            migrationBuilder.DropIndex(
                name: "IX_AppCostCentres_ProductId",
                table: "AppCostCentres");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "AppCostCentres");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "AppCostCentres",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppCostCentres_ProductId",
                table: "AppCostCentres",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCostCentres_AppProducts_ProductId",
                table: "AppCostCentres",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id");
        }
    }
}
