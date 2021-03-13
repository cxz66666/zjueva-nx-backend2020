using Microsoft.EntityFrameworkCore.Migrations;

namespace _2020_backend.Migrations
{
    public partial class addThird : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "thirdReason",
                table: "Record",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "thirdWish",
                table: "Record",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "thirdReason",
                table: "Record");

            migrationBuilder.DropColumn(
                name: "thirdWish",
                table: "Record");
        }
    }
}
