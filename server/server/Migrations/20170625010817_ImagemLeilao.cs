using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class ImagemLeilao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "usuarioId",
                table: "Leilao",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leilao_usuarioId",
                table: "Leilao",
                column: "usuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leilao_AspNetUsers_usuarioId",
                table: "Leilao",
                column: "usuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leilao_AspNetUsers_usuarioId",
                table: "Leilao");

            migrationBuilder.DropIndex(
                name: "IX_Leilao_usuarioId",
                table: "Leilao");

            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Leilao");

            migrationBuilder.DropColumn(
                name: "usuarioId",
                table: "Leilao");
        }
    }
}
