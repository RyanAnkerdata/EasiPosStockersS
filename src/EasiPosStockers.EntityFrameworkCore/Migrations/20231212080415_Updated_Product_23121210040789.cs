using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasiPosStockers.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Product_23121210040789 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CostCentreId",
                table: "AppProducts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppProducts_CostCentreId",
                table: "AppProducts",
                column: "CostCentreId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppProducts_AppCostCentres_CostCentreId",
                table: "AppProducts",
                column: "CostCentreId",
                principalTable: "AppCostCentres",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppProducts_AppCostCentres_CostCentreId",
                table: "AppProducts");

            migrationBuilder.DropIndex(
                name: "IX_AppProducts_CostCentreId",
                table: "AppProducts");

            migrationBuilder.DropColumn(
                name: "CostCentreId",
                table: "AppProducts");
        }
    }
}
