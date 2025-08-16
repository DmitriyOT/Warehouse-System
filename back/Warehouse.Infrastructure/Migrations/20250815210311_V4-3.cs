using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V43 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_balances_ResourceId",
                table: "balances");

            migrationBuilder.CreateIndex(
                name: "IX_shipmetItems_Id_ResourceId_UnitId",
                table: "shipmetItems",
                columns: new[] { "Id", "ResourceId", "UnitId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shipments_Number",
                table: "shipments",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_incomes_Number",
                table: "incomes",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_incomeItems_Id_ResourceId_UnitId",
                table: "incomeItems",
                columns: new[] { "Id", "ResourceId", "UnitId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_balances_ResourceId_UnitId",
                table: "balances",
                columns: new[] { "ResourceId", "UnitId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_shipmetItems_Id_ResourceId_UnitId",
                table: "shipmetItems");

            migrationBuilder.DropIndex(
                name: "IX_shipments_Number",
                table: "shipments");

            migrationBuilder.DropIndex(
                name: "IX_incomes_Number",
                table: "incomes");

            migrationBuilder.DropIndex(
                name: "IX_incomeItems_Id_ResourceId_UnitId",
                table: "incomeItems");

            migrationBuilder.DropIndex(
                name: "IX_balances_ResourceId_UnitId",
                table: "balances");

            migrationBuilder.CreateIndex(
                name: "IX_balances_ResourceId",
                table: "balances",
                column: "ResourceId");
        }
    }
}
