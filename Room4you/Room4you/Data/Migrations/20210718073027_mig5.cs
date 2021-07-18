using Microsoft.EntityFrameworkCore.Migrations;

namespace Room4you.Data.Migrations
{
    public partial class mig5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hoteis_Quartos_QuartoFK",
                table: "Hoteis");

            migrationBuilder.DropForeignKey(
                name: "FK_Nome_Hoteis_HotelFK",
                table: "Nome");

            migrationBuilder.DropIndex(
                name: "IX_Hoteis_QuartoFK",
                table: "Hoteis");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Nome",
                table: "Nome");

            migrationBuilder.DropColumn(
                name: "QuartoFK",
                table: "Hoteis");

            migrationBuilder.RenameTable(
                name: "Nome",
                newName: "Fotografias");

            migrationBuilder.RenameColumn(
                name: "Fotografia",
                table: "Fotografias",
                newName: "Nome");

            migrationBuilder.RenameIndex(
                name: "IX_Nome_HotelFK",
                table: "Fotografias",
                newName: "IX_Fotografias_HotelFK");

            migrationBuilder.AlterColumn<string>(
                name: "Comodidades",
                table: "Quartos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Area",
                table: "Quartos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "HotelFK",
                table: "Quartos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "Categoria",
                table: "Hoteis",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fotografias",
                table: "Fotografias",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a",
                column: "ConcurrencyStamp",
                value: "4ddd0f4e-465c-400a-91bf-6a74e0db30bb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c",
                column: "ConcurrencyStamp",
                value: "fd6d70bf-a8c8-4756-b3ac-6377e41f1d72");

            migrationBuilder.InsertData(
                table: "Fotografias",
                columns: new[] { "Id", "HotelFK", "Nome" },
                values: new object[] { 1, 1, "h1.jpg" });

            migrationBuilder.CreateIndex(
                name: "IX_Quartos_HotelFK",
                table: "Quartos",
                column: "HotelFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Fotografias_Hoteis_HotelFK",
                table: "Fotografias",
                column: "HotelFK",
                principalTable: "Hoteis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quartos_Hoteis_HotelFK",
                table: "Quartos",
                column: "HotelFK",
                principalTable: "Hoteis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fotografias_Hoteis_HotelFK",
                table: "Fotografias");

            migrationBuilder.DropForeignKey(
                name: "FK_Quartos_Hoteis_HotelFK",
                table: "Quartos");

            migrationBuilder.DropIndex(
                name: "IX_Quartos_HotelFK",
                table: "Quartos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fotografias",
                table: "Fotografias");

            migrationBuilder.DeleteData(
                table: "Fotografias",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "HotelFK",
                table: "Quartos");

            migrationBuilder.RenameTable(
                name: "Fotografias",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Nome",
                newName: "Fotografia");

            migrationBuilder.RenameIndex(
                name: "IX_Fotografias_HotelFK",
                table: "Nome",
                newName: "IX_Nome_HotelFK");

            migrationBuilder.AlterColumn<string>(
                name: "Comodidades",
                table: "Quartos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Area",
                table: "Quartos",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Categoria",
                table: "Hoteis",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "QuartoFK",
                table: "Hoteis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Nome",
                table: "Nome",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Nome_Hoteis_HotelFK",
                table: "Nome",
                column: "HotelFK",
                principalTable: "Hoteis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
