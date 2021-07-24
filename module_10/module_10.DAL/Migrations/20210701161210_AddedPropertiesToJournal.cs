using Microsoft.EntityFrameworkCore.Migrations;

namespace module_10.DAL.Migrations
{
    public partial class AddedPropertiesToJournal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasHomework",
                table: "Journal",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsAttended",
                table: "Journal",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasHomework",
                table: "Journal");

            migrationBuilder.DropColumn(
                name: "IsAttended",
                table: "Journal");
        }
    }
}
