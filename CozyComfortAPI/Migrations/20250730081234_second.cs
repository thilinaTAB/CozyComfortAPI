using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CozyComfortAPI.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlanketModels_Materials_MaterialsMaterialID",
                table: "BlanketModels");

            migrationBuilder.RenameColumn(
                name: "MaterialsMaterialID",
                table: "BlanketModels",
                newName: "MaterialID");

            migrationBuilder.RenameIndex(
                name: "IX_BlanketModels_MaterialsMaterialID",
                table: "BlanketModels",
                newName: "IX_BlanketModels_MaterialID");

            migrationBuilder.AddForeignKey(
                name: "FK_BlanketModels_Materials_MaterialID",
                table: "BlanketModels",
                column: "MaterialID",
                principalTable: "Materials",
                principalColumn: "MaterialID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlanketModels_Materials_MaterialID",
                table: "BlanketModels");

            migrationBuilder.RenameColumn(
                name: "MaterialID",
                table: "BlanketModels",
                newName: "MaterialsMaterialID");

            migrationBuilder.RenameIndex(
                name: "IX_BlanketModels_MaterialID",
                table: "BlanketModels",
                newName: "IX_BlanketModels_MaterialsMaterialID");

            migrationBuilder.AddForeignKey(
                name: "FK_BlanketModels_Materials_MaterialsMaterialID",
                table: "BlanketModels",
                column: "MaterialsMaterialID",
                principalTable: "Materials",
                principalColumn: "MaterialID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
