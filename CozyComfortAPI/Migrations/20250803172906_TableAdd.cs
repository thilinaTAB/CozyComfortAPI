using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CozyComfortAPI.Migrations
{
    /// <inheritdoc />
    public partial class TableAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Distributors",
                newName: "DistributorName");

            migrationBuilder.CreateTable(
                name: "DistributorStocks",
                columns: table => new
                {
                    DistributorStockID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    DistributorID = table.Column<int>(type: "int", nullable: false),
                    ModelID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributorStocks", x => x.DistributorStockID);
                    table.ForeignKey(
                        name: "FK_DistributorStocks_BlanketModels_ModelID",
                        column: x => x.ModelID,
                        principalTable: "BlanketModels",
                        principalColumn: "ModelID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistributorStocks_Distributors_DistributorID",
                        column: x => x.DistributorID,
                        principalTable: "Distributors",
                        principalColumn: "DistributorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistributorStocks_DistributorID",
                table: "DistributorStocks",
                column: "DistributorID");

            migrationBuilder.CreateIndex(
                name: "IX_DistributorStocks_ModelID",
                table: "DistributorStocks",
                column: "ModelID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistributorStocks");

            migrationBuilder.RenameColumn(
                name: "DistributorName",
                table: "Distributors",
                newName: "Name");
        }
    }
}
