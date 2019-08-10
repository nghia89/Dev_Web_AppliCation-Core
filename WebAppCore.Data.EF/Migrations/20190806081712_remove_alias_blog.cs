using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppCore.Data.EF.Migrations
{
    public partial class remove_alias_blog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                table: "Blogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "Blogs",
                nullable: true);
        }
    }
}
