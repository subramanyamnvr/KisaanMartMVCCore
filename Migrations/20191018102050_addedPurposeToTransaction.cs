using Microsoft.EntityFrameworkCore.Migrations;

namespace KisaanMart.Migrations
{
    public partial class addedPurposeToTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserPassword",
                table: "TempUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "Transactions");

            migrationBuilder.AlterColumn<string>(
                name: "UserPassword",
                table: "TempUsers",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
