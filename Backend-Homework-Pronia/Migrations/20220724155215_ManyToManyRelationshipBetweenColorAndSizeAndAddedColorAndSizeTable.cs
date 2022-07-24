using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend_Homework_Pronia.Migrations
{
    public partial class ManyToManyRelationshipBetweenColorAndSizeAndAddedColorAndSizeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ColorSizes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColorId = table.Column<int>(nullable: false),
                    SizeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColorSizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ColorSizes_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ColorSizes_Sizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "Sizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ColorSizes_ColorId",
                table: "ColorSizes",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ColorSizes_SizeId",
                table: "ColorSizes",
                column: "SizeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColorSizes");
        }
    }
}
