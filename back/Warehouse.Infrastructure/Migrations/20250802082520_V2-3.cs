using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_units_Name",
                table: "units",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_clients_Name",
                table: "clients",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_units_Name",
                table: "units");

            migrationBuilder.DropIndex(
                name: "IX_clients_Name",
                table: "clients");
        }
    }
}
