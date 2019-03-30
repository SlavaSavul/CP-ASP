using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CPFilmsRaiting.Migrations
{
    public partial class aaass3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmGenres_Genres_GenreId",
                table: "FilmGenres");

            migrationBuilder.DropColumn(
                name: "GenerId",
                table: "FilmGenres");

            migrationBuilder.AlterColumn<string>(
                name: "GenreId",
                table: "FilmGenres",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FilmGenres_Genres_GenreId",
                table: "FilmGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmGenres_Genres_GenreId",
                table: "FilmGenres");

            migrationBuilder.AlterColumn<string>(
                name: "GenreId",
                table: "FilmGenres",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "GenerId",
                table: "FilmGenres",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmGenres_Genres_GenreId",
                table: "FilmGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
