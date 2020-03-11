using Microsoft.EntityFrameworkCore.Migrations;

namespace KisaanMart.Migrations
{
    public partial class removedPasswordFOrUser1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserPassword",
                table: "TempUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserPassword",
                table: "TempUsers",
                nullable: false,
                defaultValue: "");
        }
    }
}
