using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class v003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DataAtualizacaoStatus",
                table: "UsuarioGameBiblioteca",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "UsuarioGameBiblioteca",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "EmProcessamento");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioGameBiblioteca_Status",
                table: "UsuarioGameBiblioteca",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UsuarioGameBiblioteca_Status",
                table: "UsuarioGameBiblioteca");

            migrationBuilder.DropColumn(
                name: "DataAtualizacaoStatus",
                table: "UsuarioGameBiblioteca");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UsuarioGameBiblioteca");
        }
    }
}
