using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend_Homework_Pronia.Migrations
{
    public partial class AddedColorsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantColors_Color_ColorId",
                table: "PlantColors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Color",
                table: "Color");

            migrationBuilder.RenameTable(
                name: "Color",
                newName: "Colors");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Size",
                maxLength: 35,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Colors",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Colors",
                table: "Colors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantColors_Colors_ColorId",
                table: "PlantColors",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantColors_Colors_ColorId",
                table: "PlantColors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Colors",
                table: "Colors");

            migrationBuilder.RenameTable(
                name: "Colors",
                newName: "Color");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Size",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 35);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Color",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Color",
                table: "Color",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantColors_Color_ColorId",
                table: "PlantColors",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
