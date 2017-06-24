using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class Nomeleilaoealteracoessolicitacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leilao_Lance_MaiorLanceId",
                table: "Leilao");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "Solicitacao",
                newName: "DataInicial");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFinal",
                table: "Solicitacao",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "MaiorLanceId",
                table: "Leilao",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Leilao",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Leilao_Lance_MaiorLanceId",
                table: "Leilao",
                column: "MaiorLanceId",
                principalTable: "Lance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leilao_Lance_MaiorLanceId",
                table: "Leilao");

            migrationBuilder.DropColumn(
                name: "DataFinal",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Leilao");

            migrationBuilder.RenameColumn(
                name: "DataInicial",
                table: "Solicitacao",
                newName: "Data");

            migrationBuilder.AlterColumn<int>(
                name: "MaiorLanceId",
                table: "Leilao",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Leilao_Lance_MaiorLanceId",
                table: "Leilao",
                column: "MaiorLanceId",
                principalTable: "Lance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
