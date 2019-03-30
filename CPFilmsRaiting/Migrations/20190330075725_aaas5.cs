using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CPFilmsRaiting.Migrations
{
    public partial class aaas5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_Films_FilmModelId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_FilmModelId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "FilmModelId",
                table: "Genres");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilmModelId",
                table: "Genres",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genres_FilmModelId",
                table: "Genres",
                column: "FilmModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_Films_FilmModelId",
                table: "Genres",
                column: "FilmModelId",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
