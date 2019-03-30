using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CPFilmsRaiting.Migrations
{
    public partial class aaas113 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmGenresModel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FilmGenresModel",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FilmId = table.Column<string>(nullable: false),
                    GenreId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmGenresModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilmGenresModel_Films_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Films",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmGenresModel_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmGenresModel_FilmId",
                table: "FilmGenresModel",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmGenresModel_GenreId",
                table: "FilmGenresModel",
                column: "GenreId");
        }
    }
}
