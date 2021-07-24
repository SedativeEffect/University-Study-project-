using Microsoft.EntityFrameworkCore.Migrations;

namespace module_10.DAL.Migrations
{
    public partial class LectureNameProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LectureName",
                table: "Lectures",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LectureName",
                table: "Lectures");
        }
    }
}
