using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class usuarioSolicitacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Solicitacao",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacao_UsuarioId",
                table: "Solicitacao",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solicitacao_AspNetUsers_UsuarioId",
                table: "Solicitacao",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solicitacao_AspNetUsers_UsuarioId",
                table: "Solicitacao");

            migrationBuilder.DropIndex(
                name: "IX_Solicitacao_UsuarioId",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Solicitacao");
        }
    }
}
