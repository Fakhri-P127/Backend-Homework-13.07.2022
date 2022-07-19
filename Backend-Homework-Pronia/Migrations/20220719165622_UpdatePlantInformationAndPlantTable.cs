using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend_Homework_Pronia.Migrations
{
    public partial class UpdatePlantInformationAndPlantTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Definition",
                table: "PlantInformations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Definition",
                table: "PlantInformations");
        }
    }
}
