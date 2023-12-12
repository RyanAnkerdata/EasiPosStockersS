using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasiPosStockers.Migrations
{
    /// <inheritdoc />
    public partial class Updated_CostCentre_23121210201180 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCostCentreBranch_AppCostCentres_CostCentreId",
                table: "AppCostCentreBranch");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_AppCostCentreBranch_AppCostCentres_CostCentreId",
                table: "AppCostCentreBranch",
                column: "CostCentreId",
                principalTable: "AppCostCentres",
                principalColumn: "Id");
        }
    }
}
