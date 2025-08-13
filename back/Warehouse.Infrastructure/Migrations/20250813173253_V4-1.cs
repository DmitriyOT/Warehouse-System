using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Warehouse.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V41 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shipmetItems_incomes_IncomeId",
                table: "shipmetItems");

            migrationBuilder.RenameColumn(
                name: "IncomeId",
                table: "shipmetItems",
                newName: "ShipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_shipmetItems_IncomeId",
                table: "shipmetItems",
                newName: "IX_shipmetItems_ShipmentId");

            migrationBuilder.AddColumn<long>(
                name: "ClientId",
                table: "shipments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "shipments",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "shipments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_shipments_ClientId",
                table: "shipments",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_shipments_clients_ClientId",
                table: "shipments",
                column: "ClientId",
                principalTable: "clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_shipmetItems_shipments_ShipmentId",
                table: "shipmetItems",
                column: "ShipmentId",
                principalTable: "shipments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shipments_clients_ClientId",
                table: "shipments");

            migrationBuilder.DropForeignKey(
                name: "FK_shipmetItems_shipments_ShipmentId",
                table: "shipmetItems");

            migrationBuilder.DropIndex(
                name: "IX_shipments_ClientId",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "shipments");

            migrationBuilder.RenameColumn(
                name: "ShipmentId",
                table: "shipmetItems",
                newName: "IncomeId");

            migrationBuilder.RenameIndex(
                name: "IX_shipmetItems_ShipmentId",
                table: "shipmetItems",
                newName: "IX_shipmetItems_IncomeId");

            migrationBuilder.AddForeignKey(
                name: "FK_shipmetItems_incomes_IncomeId",
                table: "shipmetItems",
                column: "IncomeId",
                principalTable: "incomes",
                principalColumn: "Id");
        }
    }
}
