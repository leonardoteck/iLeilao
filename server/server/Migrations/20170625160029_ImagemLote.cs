using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class ImagemLote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Leilao");

            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "Lote",
                type: "varchar",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Lote");

            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "Solicitacao",
                type: "varchar",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "Leilao",
                type: "varchar",
                nullable: true);
        }
    }
}
