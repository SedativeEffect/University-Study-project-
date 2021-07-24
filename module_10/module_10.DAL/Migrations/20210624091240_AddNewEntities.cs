using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace module_10.DAL.Migrations
{
    public partial class AddNewEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<int>(type: "int", nullable: false),
                    LectureDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Journal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LectureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Mark = table.Column<int>(type: "int", nullable: false),
                    HasHomework = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Journal_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Journal_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LectureUser",
                columns: table => new
                {
                    LecturesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LectureUser", x => new { x.LecturesId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_LectureUser_AspNetUsers_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LectureUser_Lectures_LecturesId",
                        column: x => x.LecturesId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Journal_LectureId",
                table: "Journal",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_Journal_StudentId",
                table: "Journal",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_LectureUser_StudentsId",
                table: "LectureUser",
                column: "StudentsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Journal");

            migrationBuilder.DropTable(
                name: "LectureUser");

            migrationBuilder.DropTable(
                name: "Lectures");
        }
    }
}
