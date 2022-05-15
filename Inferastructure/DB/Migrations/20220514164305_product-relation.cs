using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inferastructure.Migrations
{
    public partial class productrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_categories_categoryId",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "categoryId",
                table: "products",
                newName: "CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_products_categoryId",
                table: "products",
                newName: "IX_products_CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_products_categories_CategoryID",
                table: "products",
                column: "CategoryID",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_categories_CategoryID",
                table: "products");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "products",
                newName: "categoryId");

            migrationBuilder.RenameIndex(
                name: "IX_products_CategoryID",
                table: "products",
                newName: "IX_products_categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_products_categories_categoryId",
                table: "products",
                column: "categoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
     
}
