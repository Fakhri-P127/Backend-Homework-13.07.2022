using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend_Homework_Pronia.Migrations
{
    public partial class ChangingRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlantSizes");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateTable(
                name: "PlantSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantId = table.Column<int>(type: "int", nullable: false),
                    SizeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantSizes_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlantSizes_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlantSizes_PlantId",
                table: "PlantSizes",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantSizes_SizeId",
                table: "PlantSizes",
                column: "SizeId");
        }
    }
}
