using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasiPosStockers.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Branch_23121213451623 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppBranches_AppCostCentres_CostCentreId",
                table: "AppBranches");

            migrationBuilder.DropIndex(
                name: "IX_AppBranches_CostCentreId",
                table: "AppBranches");

            migrationBuilder.DropColumn(
                name: "CostCentreId",
                table: "AppBranches");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CostCentreId",
                table: "AppBranches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppBranches_CostCentreId",
                table: "AppBranches",
                column: "CostCentreId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppBranches_AppCostCentres_CostCentreId",
                table: "AppBranches",
                column: "CostCentreId",
                principalTable: "AppCostCentres",
                principalColumn: "Id");
        }
    }
}
