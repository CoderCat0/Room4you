using Microsoft.EntityFrameworkCore.Migrations;

namespace Room4you.Data.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hoteis_Clientes_IdFotoFK",
                table: "Hoteis");

            migrationBuilder.DropIndex(
                name: "IX_Hoteis_IdFotoFK",
                table: "Hoteis");

            migrationBuilder.DropColumn(
                name: "IdFotoFK",
                table: "Hoteis");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Hoteis",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fotografia",
                table: "Nome",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a",
                column: "ConcurrencyStamp",
                value: "a98b0b76-4e8f-44de-a57f-5c160065f2c2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c",
                column: "ConcurrencyStamp",
                value: "68d0697e-14dc-4083-8f05-6f4e507ca89e");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Hoteis");

            migrationBuilder.DropColumn(
                name: "Fotografia",
                table: "Nome");

            migrationBuilder.AddColumn<int>(
                name: "IdFotoFK",
                table: "Hoteis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a",
                column: "ConcurrencyStamp",
                value: "20178c93-e699-4942-a1c4-cb0da719bd78");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c",
                column: "ConcurrencyStamp",
                value: "159a789b-9fbb-4b69-9dc6-ce35fed335d2");

            migrationBuilder.CreateIndex(
                name: "IX_Hoteis_IdFotoFK",
                table: "Hoteis",
                column: "IdFotoFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Hoteis_Clientes_IdFotoFK",
                table: "Hoteis",
                column: "IdFotoFK",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
