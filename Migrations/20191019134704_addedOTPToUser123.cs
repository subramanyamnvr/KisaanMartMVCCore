using Microsoft.EntityFrameworkCore.Migrations;

namespace KisaanMart.Migrations
{
    public partial class addedOTPToUser123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "behalfuserId",
                table: "Transactions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "OTP",
                table: "LoginViewModel",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "behalfuserId",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OTP",
                table: "LoginViewModel",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
