using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class DiasDuracao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataFinal",
                table: "Solicitacao");

            migrationBuilder.DropColumn(
                name: "DataInicial",
                table: "Solicitacao");

            migrationBuilder.AddColumn<int>(
                name: "DiasDuracao",
                table: "Solicitacao",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiasDuracao",
                table: "Solicitacao");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFinal",
                table: "Solicitacao",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInicial",
                table: "Solicitacao",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
