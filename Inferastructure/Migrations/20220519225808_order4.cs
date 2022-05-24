using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inferastructure.Migrations
{
    public partial class order4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderItem",
                newName: "ProductIdInDb");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductIdInDb",
                table: "OrderItem",
                newName: "ProductId");
        }
    }
}
