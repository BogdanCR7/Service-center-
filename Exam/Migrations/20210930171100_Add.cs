using Microsoft.EntityFrameworkCore.Migrations;

namespace Exam.Migrations
{
    public partial class Add : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Languge",
                table: "AspNetUsers",
                newName: "Language");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Language",
                table: "AspNetUsers",
                newName: "Languge");
        }
    }
}
