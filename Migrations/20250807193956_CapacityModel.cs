using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CozyComfortAPI.Migrations
{
    /// <inheritdoc />
    public partial class CapacityModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CapacityPerDay",
                table: "BlanketModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SellerInventories_BlanketModelId",
                table: "SellerInventories",
                column: "BlanketModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellerInventories_BlanketModels_BlanketModelId",
                table: "SellerInventories",
                column: "BlanketModelId",
                principalTable: "BlanketModels",
                principalColumn: "ModelID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellerInventories_BlanketModels_BlanketModelId",
                table: "SellerInventories");

            migrationBuilder.DropIndex(
                name: "IX_SellerInventories_BlanketModelId",
                table: "SellerInventories");

            migrationBuilder.DropColumn(
                name: "CapacityPerDay",
                table: "BlanketModels");
        }
    }
}
