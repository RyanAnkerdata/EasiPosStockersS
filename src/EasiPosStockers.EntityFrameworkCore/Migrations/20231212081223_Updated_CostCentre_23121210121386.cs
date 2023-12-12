using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasiPosStockers.Migrations
{
    /// <inheritdoc />
    public partial class Updated_CostCentre_23121210121386 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppCostCentreBranch",
                columns: table => new
                {
                    CostCentreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCostCentreBranch", x => new { x.CostCentreId, x.BranchId });
                    table.ForeignKey(
                        name: "FK_AppCostCentreBranch_AppBranches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "AppBranches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AppCostCentreBranch_AppCostCentres_CostCentreId",
                        column: x => x.CostCentreId,
                        principalTable: "AppCostCentres",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppCostCentreBranch_BranchId",
                table: "AppCostCentreBranch",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCostCentreBranch_CostCentreId_BranchId",
                table: "AppCostCentreBranch",
                columns: new[] { "CostCentreId", "BranchId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppCostCentreBranch");
        }
    }
}
