using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KisaanMart.Migrations
{
    public partial class workeradded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserPhoneNo",
                table: "Users",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 13);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "TempUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "TempUsers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "TempUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: false),
                    UserPhoneNo = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "TempUsers");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "TempUsers");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TempUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserPhoneNo",
                table: "Users",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
