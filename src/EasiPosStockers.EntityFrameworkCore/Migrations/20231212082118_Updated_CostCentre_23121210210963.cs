using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasiPosStockers.Migrations
{
    /// <inheritdoc />
    public partial class Updated_CostCentre_23121210210963 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BranchId",
                table: "AppCostCentres",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppCostCentres_BranchId",
                table: "AppCostCentres",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCostCentres_AppBranches_BranchId",
                table: "AppCostCentres",
                column: "BranchId",
                principalTable: "AppBranches",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCostCentres_AppBranches_BranchId",
                table: "AppCostCentres");

            migrationBuilder.DropIndex(
                name: "IX_AppCostCentres_BranchId",
                table: "AppCostCentres");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "AppCostCentres");
        }
    }
}
