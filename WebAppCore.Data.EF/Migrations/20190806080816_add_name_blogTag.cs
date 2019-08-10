using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppCore.Data.EF.Migrations
{
    public partial class add_name_blogTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BlogTags",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "BlogTags");
        }
    }
}
