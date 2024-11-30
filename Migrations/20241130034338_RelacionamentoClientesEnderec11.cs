using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetoihc.Migrations
{
    /// <inheritdoc />
    public partial class RelacionamentoClientesEnderec11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnderecoId1",
                table: "Clientes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_EnderecoId1",
                table: "Clientes",
                column: "EnderecoId1",
                unique: true,
                filter: "[EnderecoId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_Enderecos_EnderecoId1",
                table: "Clientes",
                column: "EnderecoId1",
                principalTable: "Enderecos",
                principalColumn: "EnderecoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_Enderecos_EnderecoId1",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_EnderecoId1",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "EnderecoId1",
                table: "Clientes");
        }
    }
}
