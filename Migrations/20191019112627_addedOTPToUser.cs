using Microsoft.EntityFrameworkCore.Migrations;

namespace KisaanMart.Migrations
{
    public partial class addedOTPToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserPassword",
                table: "LoginViewModel",
                newName: "OTP");

            migrationBuilder.AddColumn<int>(
                name: "RandOTP",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RandOTP",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "OTP",
                table: "LoginViewModel",
                newName: "UserPassword");
        }
    }
}
