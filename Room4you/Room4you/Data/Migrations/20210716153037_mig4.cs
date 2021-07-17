using Microsoft.EntityFrameworkCore.Migrations;

namespace Room4you.Data.Migrations
{
    public partial class mig4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Compras_Quartos_IdQuartoFK",
                table: "Compras");

            migrationBuilder.DropIndex(
                name: "IX_Compras_IdQuartoFK",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "IdQuartoFK",
                table: "Compras");

            migrationBuilder.AddColumn<int>(
                name: "QuartoFK",
                table: "Hoteis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Nif",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a",
                column: "ConcurrencyStamp",
                value: "c483589d-12ba-4630-8085-48a459295c3c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c",
                column: "ConcurrencyStamp",
                value: "c8e60be8-d46e-4ad6-9ab6-0fa281f1b28c");

            migrationBuilder.CreateIndex(
                name: "IX_Hoteis_QuartoFK",
                table: "Hoteis",
                column: "QuartoFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Hoteis_Quartos_QuartoFK",
                table: "Hoteis",
                column: "QuartoFK",
                principalTable: "Quartos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hoteis_Quartos_QuartoFK",
                table: "Hoteis");

            migrationBuilder.DropIndex(
                name: "IX_Hoteis_QuartoFK",
                table: "Hoteis");

            migrationBuilder.DropColumn(
                name: "QuartoFK",
                table: "Hoteis");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Clientes");

            migrationBuilder.AddColumn<int>(
                name: "IdQuartoFK",
                table: "Compras",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Nif",
                table: "Clientes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a",
                column: "ConcurrencyStamp",
                value: "8881b232-e1c7-47df-aa1a-24589bdb0011");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c",
                column: "ConcurrencyStamp",
                value: "88557f8e-73b1-4f65-8c76-c9e4138ea916");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_IdQuartoFK",
                table: "Compras",
                column: "IdQuartoFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Compras_Quartos_IdQuartoFK",
                table: "Compras",
                column: "IdQuartoFK",
                principalTable: "Quartos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
