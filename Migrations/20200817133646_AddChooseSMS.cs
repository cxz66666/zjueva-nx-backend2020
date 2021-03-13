using Microsoft.EntityFrameworkCore.Migrations;

namespace _2020_backend.Migrations
{
    public partial class AddChooseSMS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SendSMS",
                table: "Time",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendSMS",
                table: "Time");
        }
    }
}
