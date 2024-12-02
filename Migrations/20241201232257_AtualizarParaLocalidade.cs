using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projetoihc.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarParaLocalidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cidade",
                table: "Enderecos",
                newName: "Localidade");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Localidade",
                table: "Enderecos",
                newName: "Cidade");
        }
    }
}
