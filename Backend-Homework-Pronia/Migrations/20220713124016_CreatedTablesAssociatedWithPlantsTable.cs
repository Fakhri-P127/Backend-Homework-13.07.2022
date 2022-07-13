using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend_Homework_Pronia.Migrations
{
    public partial class CreatedTablesAssociatedWithPlantsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantCategory_Category_CategoryId",
                table: "PlantCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantCategory_Plants_PlantId",
                table: "PlantCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantColor_Color_ColorId",
                table: "PlantColor");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantColor_Plants_PlantId",
                table: "PlantColor");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantImage_Plants_PlantId",
                table: "PlantImage");

            migrationBuilder.DropForeignKey(
                name: "FK_Plants_PlantInformation_PlantInformationId",
                table: "Plants");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantSize_Plants_PlantId",
                table: "PlantSize");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantSize_Size_SizeId",
                table: "PlantSize");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantSize",
                table: "PlantSize");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantInformation",
                table: "PlantInformation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantImage",
                table: "PlantImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantColor",
                table: "PlantColor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantCategory",
                table: "PlantCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "PlantSize",
                newName: "PlantSizes");

            migrationBuilder.RenameTable(
                name: "PlantInformation",
                newName: "PlantInformations");

            migrationBuilder.RenameTable(
                name: "PlantImage",
                newName: "PlantImages");

            migrationBuilder.RenameTable(
                name: "PlantColor",
                newName: "PlantColors");

            migrationBuilder.RenameTable(
                name: "PlantCategory",
                newName: "PlantCategories");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_PlantSize_SizeId",
                table: "PlantSizes",
                newName: "IX_PlantSizes_SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_PlantSize_PlantId",
                table: "PlantSizes",
                newName: "IX_PlantSizes_PlantId");

            migrationBuilder.RenameIndex(
                name: "IX_PlantImage_PlantId",
                table: "PlantImages",
                newName: "IX_PlantImages_PlantId");

            migrationBuilder.RenameIndex(
                name: "IX_PlantColor_PlantId",
                table: "PlantColors",
                newName: "IX_PlantColors_PlantId");

            migrationBuilder.RenameIndex(
                name: "IX_PlantColor_ColorId",
                table: "PlantColors",
                newName: "IX_PlantColors_ColorId");

            migrationBuilder.RenameIndex(
                name: "IX_PlantCategory_PlantId",
                table: "PlantCategories",
                newName: "IX_PlantCategories_PlantId");

            migrationBuilder.RenameIndex(
                name: "IX_PlantCategory_CategoryId",
                table: "PlantCategories",
                newName: "IX_PlantCategories_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantSizes",
                table: "PlantSizes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantInformations",
                table: "PlantInformations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantImages",
                table: "PlantImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantColors",
                table: "PlantColors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantCategories",
                table: "PlantCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantCategories_Categories_CategoryId",
                table: "PlantCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantCategories_Plants_PlantId",
                table: "PlantCategories",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantColors_Color_ColorId",
                table: "PlantColors",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantColors_Plants_PlantId",
                table: "PlantColors",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantImages_Plants_PlantId",
                table: "PlantImages",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Plants_PlantInformations_PlantInformationId",
                table: "Plants",
                column: "PlantInformationId",
                principalTable: "PlantInformations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantSizes_Plants_PlantId",
                table: "PlantSizes",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantSizes_Size_SizeId",
                table: "PlantSizes",
                column: "SizeId",
                principalTable: "Size",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlantCategories_Categories_CategoryId",
                table: "PlantCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantCategories_Plants_PlantId",
                table: "PlantCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantColors_Color_ColorId",
                table: "PlantColors");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantColors_Plants_PlantId",
                table: "PlantColors");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantImages_Plants_PlantId",
                table: "PlantImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Plants_PlantInformations_PlantInformationId",
                table: "Plants");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantSizes_Plants_PlantId",
                table: "PlantSizes");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantSizes_Size_SizeId",
                table: "PlantSizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantSizes",
                table: "PlantSizes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantInformations",
                table: "PlantInformations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantImages",
                table: "PlantImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantColors",
                table: "PlantColors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlantCategories",
                table: "PlantCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "PlantSizes",
                newName: "PlantSize");

            migrationBuilder.RenameTable(
                name: "PlantInformations",
                newName: "PlantInformation");

            migrationBuilder.RenameTable(
                name: "PlantImages",
                newName: "PlantImage");

            migrationBuilder.RenameTable(
                name: "PlantColors",
                newName: "PlantColor");

            migrationBuilder.RenameTable(
                name: "PlantCategories",
                newName: "PlantCategory");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_PlantSizes_SizeId",
                table: "PlantSize",
                newName: "IX_PlantSize_SizeId");

            migrationBuilder.RenameIndex(
                name: "IX_PlantSizes_PlantId",
                table: "PlantSize",
                newName: "IX_PlantSize_PlantId");

            migrationBuilder.RenameIndex(
                name: "IX_PlantImages_PlantId",
                table: "PlantImage",
                newName: "IX_PlantImage_PlantId");

            migrationBuilder.RenameIndex(
                name: "IX_PlantColors_PlantId",
                table: "PlantColor",
                newName: "IX_PlantColor_PlantId");

            migrationBuilder.RenameIndex(
                name: "IX_PlantColors_ColorId",
                table: "PlantColor",
                newName: "IX_PlantColor_ColorId");

            migrationBuilder.RenameIndex(
                name: "IX_PlantCategories_PlantId",
                table: "PlantCategory",
                newName: "IX_PlantCategory_PlantId");

            migrationBuilder.RenameIndex(
                name: "IX_PlantCategories_CategoryId",
                table: "PlantCategory",
                newName: "IX_PlantCategory_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantSize",
                table: "PlantSize",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantInformation",
                table: "PlantInformation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantImage",
                table: "PlantImage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantColor",
                table: "PlantColor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlantCategory",
                table: "PlantCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlantCategory_Category_CategoryId",
                table: "PlantCategory",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantCategory_Plants_PlantId",
                table: "PlantCategory",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantColor_Color_ColorId",
                table: "PlantColor",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantColor_Plants_PlantId",
                table: "PlantColor",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantImage_Plants_PlantId",
                table: "PlantImage",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Plants_PlantInformation_PlantInformationId",
                table: "Plants",
                column: "PlantInformationId",
                principalTable: "PlantInformation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantSize_Plants_PlantId",
                table: "PlantSize",
                column: "PlantId",
                principalTable: "Plants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlantSize_Size_SizeId",
                table: "PlantSize",
                column: "SizeId",
                principalTable: "Size",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
