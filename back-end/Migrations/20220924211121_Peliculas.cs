using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace back_end.Migrations
{
    public partial class Peliculas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Peliculas",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    resumen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    trailer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    enCines = table.Column<bool>(type: "bit", nullable: false),
                    fechaLanzamiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    poster = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peliculas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PeliculasActores",
                columns: table => new
                {
                    peliculaID = table.Column<int>(type: "int", nullable: false),
                    actorID = table.Column<int>(type: "int", nullable: false),
                    personaje = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeliculasActores", x => new { x.actorID, x.peliculaID });
                    table.ForeignKey(
                        name: "FK_PeliculasActores_Actores_actorID",
                        column: x => x.actorID,
                        principalTable: "Actores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeliculasActores_Peliculas_peliculaID",
                        column: x => x.peliculaID,
                        principalTable: "Peliculas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeliculasCines",
                columns: table => new
                {
                    peliculaID = table.Column<int>(type: "int", nullable: false),
                    cineID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeliculasCines", x => new { x.peliculaID, x.cineID });
                    table.ForeignKey(
                        name: "FK_PeliculasCines_Cines_cineID",
                        column: x => x.cineID,
                        principalTable: "Cines",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeliculasCines_Peliculas_peliculaID",
                        column: x => x.peliculaID,
                        principalTable: "Peliculas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeliculasGeneros",
                columns: table => new
                {
                    peliculaID = table.Column<int>(type: "int", nullable: false),
                    generoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeliculasGeneros", x => new { x.peliculaID, x.generoID });
                    table.ForeignKey(
                        name: "FK_PeliculasGeneros_Generos_generoID",
                        column: x => x.generoID,
                        principalTable: "Generos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeliculasGeneros_Peliculas_peliculaID",
                        column: x => x.peliculaID,
                        principalTable: "Peliculas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PeliculasActores_peliculaID",
                table: "PeliculasActores",
                column: "peliculaID");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculasCines_cineID",
                table: "PeliculasCines",
                column: "cineID");

            migrationBuilder.CreateIndex(
                name: "IX_PeliculasGeneros_generoID",
                table: "PeliculasGeneros",
                column: "generoID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeliculasActores");

            migrationBuilder.DropTable(
                name: "PeliculasCines");

            migrationBuilder.DropTable(
                name: "PeliculasGeneros");

            migrationBuilder.DropTable(
                name: "Peliculas");
        }
    }
}
