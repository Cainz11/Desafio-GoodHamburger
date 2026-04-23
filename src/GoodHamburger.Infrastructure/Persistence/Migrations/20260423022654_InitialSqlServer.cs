using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GoodHamburger.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialSqlServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "menu_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menu_items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "order_lines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_lines_menu_items_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "menu_items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_order_lines_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "menu_items",
                columns: new[] { "Id", "Name", "Role", "UnitPrice" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111101"), "X Burger", 1, 5.00m },
                    { new Guid("11111111-1111-1111-1111-111111111102"), "X Egg", 1, 4.50m },
                    { new Guid("11111111-1111-1111-1111-111111111103"), "X Bacon", 1, 7.00m },
                    { new Guid("22222222-2222-2222-2222-222222222201"), "Batata frita", 2, 2.00m },
                    { new Guid("33333333-3333-3333-3333-333333333301"), "Refrigerante", 3, 2.50m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_order_lines_MenuItemId",
                table: "order_lines",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_order_lines_OrderId_MenuItemId",
                table: "order_lines",
                columns: new[] { "OrderId", "MenuItemId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_lines");

            migrationBuilder.DropTable(
                name: "menu_items");

            migrationBuilder.DropTable(
                name: "orders");
        }
    }
}
