using Microsoft.EntityFrameworkCore.Migrations;

namespace KisaanMart.Migrations
{
    public partial class removedPasswordFOrUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserPassword",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserPassword",
                table: "Users",
                nullable: true);
        }
    }
}
