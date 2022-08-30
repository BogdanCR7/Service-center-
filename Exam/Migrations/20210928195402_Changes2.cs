using Microsoft.EntityFrameworkCore.Migrations;

namespace Exam.Migrations
{
    public partial class Changes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_clients_ClientsId",
                table: "orders");

            migrationBuilder.AlterColumn<int>(
                name: "ClientsId",
                table: "orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_orders_clients_ClientsId",
                table: "orders",
                column: "ClientsId",
                principalTable: "clients",
                principalColumn: "ClientsId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orders_clients_ClientsId",
                table: "orders");

            migrationBuilder.AlterColumn<int>(
                name: "ClientsId",
                table: "orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_orders_clients_ClientsId",
                table: "orders",
                column: "ClientsId",
                principalTable: "clients",
                principalColumn: "ClientsId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
