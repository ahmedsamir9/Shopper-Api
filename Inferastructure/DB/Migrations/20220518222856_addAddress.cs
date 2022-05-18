using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inferastructure.Migrations
{
    public partial class addAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShippedAddress_City",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippedAddress_Detailed",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippedAddress_State",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShippedAddress_Street",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippedAddress_City",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippedAddress_Detailed",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippedAddress_State",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippedAddress_Street",
                table: "Orders");
        }
    }
}
