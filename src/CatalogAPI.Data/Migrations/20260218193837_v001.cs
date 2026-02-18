using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class v001 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Desenvolvedor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataCriacao = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DataRelease = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioGameBiblioteca",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoAquisicao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrecoAquisicao = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataAquisicao = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioGameBiblioteca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioGameBiblioteca_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioGameBiblioteca_GameId",
                table: "UsuarioGameBiblioteca",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioGameBiblioteca_UsuarioId_GameId",
                table: "UsuarioGameBiblioteca",
                columns: new[] { "UsuarioId", "GameId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuarioGameBiblioteca");

            migrationBuilder.DropTable(
                name: "Game");
        }
    }
}
