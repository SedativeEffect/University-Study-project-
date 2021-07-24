using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace module_10.DAL.Migrations
{
    public partial class AddHomework : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasHomework",
                table: "Journal");

            migrationBuilder.CreateTable(
                name: "Homeworks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LectureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Task = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homeworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Homeworks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Homeworks_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_LectureId",
                table: "Homeworks",
                column: "LectureId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_UserId",
                table: "Homeworks",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Homeworks");

            migrationBuilder.AddColumn<bool>(
                name: "HasHomework",
                table: "Journal",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
