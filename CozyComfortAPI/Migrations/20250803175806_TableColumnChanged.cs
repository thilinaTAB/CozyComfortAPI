using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CozyComfortAPI.Migrations
{
    /// <inheritdoc />
    public partial class TableColumnChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "DistributorStocks",
                newName: "Inventory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Inventory",
                table: "DistributorStocks",
                newName: "Stock");
        }
    }
}
