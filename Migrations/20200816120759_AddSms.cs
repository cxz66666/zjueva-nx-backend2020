using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace _2020_backend.Migrations
{
    public partial class AddSms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OperatorId = table.Column<string>(nullable: false),
                    OperatorName = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    AddTime = table.Column<DateTime>(nullable: false),
                    RecordId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Record",
                columns: table => new
                {
                    rid = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(nullable: true),
                    id_student = table.Column<string>(nullable: true),
                    sex = table.Column<bool>(nullable: false),
                    grade = table.Column<int>(nullable: false),
                    major = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    phone = table.Column<string>(nullable: true),
                    firstWish = table.Column<int>(nullable: false),
                    secondWish = table.Column<int>(nullable: false),
                    adjustment = table.Column<bool>(nullable: false),
                    firstReason = table.Column<string>(nullable: true),
                    secondReason = table.Column<string>(nullable: true),
                    question1 = table.Column<string>(nullable: true),
                    question2 = table.Column<string>(nullable: true),
                    strguid = table.Column<string>(nullable: true),
                    Times = table.Column<List<int>>(nullable: true),
                    addedDate = table.Column<DateTime>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    ip = table.Column<string>(nullable: true),
                    InterviewID = table.Column<int>(nullable: false),
                    InterviewTime = table.Column<string>(nullable: true),
                    FinalResult = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Record", x => x.rid);
                });

            migrationBuilder.CreateTable(
                name: "Sms",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_student = table.Column<int>(nullable: false),
                    sendTime = table.Column<DateTime>(nullable: false),
                    type = table.Column<string>(nullable: true),
                    OperatorName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Time",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Day = table.Column<string>(nullable: false),
                    BeginTime = table.Column<string>(nullable: false),
                    Place = table.Column<string>(nullable: false),
                    Chief = table.Column<string>(nullable: true),
                    TakenNum = table.Column<int>(nullable: false),
                    NowNum = table.Column<int>(nullable: false),
                    Students = table.Column<List<int>>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Time", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Uid = table.Column<string>(nullable: false),
                    stuID = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Secret = table.Column<string>(nullable: true),
                    isManager = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Uid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "Record");

            migrationBuilder.DropTable(
                name: "Sms");

            migrationBuilder.DropTable(
                name: "Time");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
