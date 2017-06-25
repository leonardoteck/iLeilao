using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class correcaoUsuarioId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leilao_AspNetUsers_usuarioId",
                table: "Leilao");

            migrationBuilder.RenameColumn(
                name: "usuarioId",
                table: "Leilao",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Leilao_usuarioId",
                table: "Leilao",
                newName: "IX_Leilao_UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leilao_AspNetUsers_UsuarioId",
                table: "Leilao",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leilao_AspNetUsers_UsuarioId",
                table: "Leilao");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Leilao",
                newName: "usuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Leilao_UsuarioId",
                table: "Leilao",
                newName: "IX_Leilao_usuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leilao_AspNetUsers_usuarioId",
                table: "Leilao",
                column: "usuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
