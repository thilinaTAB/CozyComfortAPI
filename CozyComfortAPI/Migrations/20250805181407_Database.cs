using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CozyComfortAPI.Migrations
{
    /// <inheritdoc />
    public partial class Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Distributors_ByDistributorDistributorID",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "BlanketModelOrder");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ByDistributorDistributorID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ByDistributorDistributorID",
                table: "Orders",
                newName: "Quantity");

            migrationBuilder.AddColumn<int>(
                name: "DistributorID",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModelID",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DistributorID",
                table: "Orders",
                column: "DistributorID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ModelID",
                table: "Orders",
                column: "ModelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_BlanketModels_ModelID",
                table: "Orders",
                column: "ModelID",
                principalTable: "BlanketModels",
                principalColumn: "ModelID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Distributors_DistributorID",
                table: "Orders",
                column: "DistributorID",
                principalTable: "Distributors",
                principalColumn: "DistributorID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_BlanketModels_ModelID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Distributors_DistributorID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DistributorID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ModelID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DistributorID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ModelID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Orders",
                newName: "ByDistributorDistributorID");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "BlanketModelOrder",
                columns: table => new
                {
                    BlanketModelsModelID = table.Column<int>(type: "int", nullable: false),
                    OrdersOrderID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlanketModelOrder", x => new { x.BlanketModelsModelID, x.OrdersOrderID });
                    table.ForeignKey(
                        name: "FK_BlanketModelOrder_BlanketModels_BlanketModelsModelID",
                        column: x => x.BlanketModelsModelID,
                        principalTable: "BlanketModels",
                        principalColumn: "ModelID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlanketModelOrder_Orders_OrdersOrderID",
                        column: x => x.OrdersOrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ByDistributorDistributorID",
                table: "Orders",
                column: "ByDistributorDistributorID");

            migrationBuilder.CreateIndex(
                name: "IX_BlanketModelOrder_OrdersOrderID",
                table: "BlanketModelOrder",
                column: "OrdersOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Distributors_ByDistributorDistributorID",
                table: "Orders",
                column: "ByDistributorDistributorID",
                principalTable: "Distributors",
                principalColumn: "DistributorID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
