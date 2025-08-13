using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_incomeItems_incomes_IncomeId",
                table: "incomeItems");

            migrationBuilder.AlterColumn<long>(
                name: "IncomeId",
                table: "incomeItems",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_incomeItems_incomes_IncomeId",
                table: "incomeItems",
                column: "IncomeId",
                principalTable: "incomes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_incomeItems_incomes_IncomeId",
                table: "incomeItems");

            migrationBuilder.AlterColumn<long>(
                name: "IncomeId",
                table: "incomeItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_incomeItems_incomes_IncomeId",
                table: "incomeItems",
                column: "IncomeId",
                principalTable: "incomes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
