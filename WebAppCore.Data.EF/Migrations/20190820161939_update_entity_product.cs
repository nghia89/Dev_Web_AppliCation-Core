using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppCore.Data.EF.Migrations
{
    public partial class update_entity_product : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProductPlashSale",
                table: "Product",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProductPlashSale",
                table: "Product");
        }
    }
}
