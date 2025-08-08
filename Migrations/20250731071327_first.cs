using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CozyComfortAPI.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Distributors",
                columns: table => new
                {
                    DistributorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distributors", x => x.DistributorID);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    MaterialID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.MaterialID);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ByDistributorDistributorID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_Distributors_ByDistributorDistributorID",
                        column: x => x.ByDistributorDistributorID,
                        principalTable: "Distributors",
                        principalColumn: "DistributorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlanketModels",
                columns: table => new
                {
                    ModelID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    MaterialID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlanketModels", x => x.ModelID);
                    table.ForeignKey(
                        name: "FK_BlanketModels_Materials_MaterialID",
                        column: x => x.MaterialID,
                        principalTable: "Materials",
                        principalColumn: "MaterialID",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                table: "Materials",
                columns: new[] { "MaterialID", "Description", "MaterialName" },
                values: new object[,]
                {
                    { 1, "100% organic cotton", "Cotton" },
                    { 2, "Soft merino wool", "Wool" },
                    { 3, "Warm polyester fleece", "Fleece" },
                    { 4, "Eco-friendly bamboo fiber", "Bamboo" },
                    { 5, "Luxurious cashmere wool", "Cashmere" },
                    { 6, "Soft chenille fabric", "Chenille" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlanketModelOrder_OrdersOrderID",
                table: "BlanketModelOrder",
                column: "OrdersOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_BlanketModels_MaterialID",
                table: "BlanketModels",
                column: "MaterialID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ByDistributorDistributorID",
                table: "Orders",
                column: "ByDistributorDistributorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlanketModelOrder");

            migrationBuilder.DropTable(
                name: "BlanketModels");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Distributors");
        }
    }
}
