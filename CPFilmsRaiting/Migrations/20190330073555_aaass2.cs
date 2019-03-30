using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CPFilmsRaiting.Migrations
{
    public partial class aaass2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GenersId",
                table: "FilmGenres",
                newName: "GenerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GenerId",
                table: "FilmGenres",
                newName: "GenersId");
        }
    }
}
