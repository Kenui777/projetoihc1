using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetoihc.Migrations
{
    /// <inheritdoc />
    public partial class ultimans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Enderecos");

            migrationBuilder.AlterColumn<string>(
                name: "Logradouro",
                table: "Enderecos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Complemento",
                table: "Enderecos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "CEP",
                table: "Enderecos",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(9)",
                oldMaxLength: 9);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Logradouro",
                table: "Enderecos",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Complemento",
                table: "Enderecos",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "CEP",
                table: "Enderecos",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Enderecos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
