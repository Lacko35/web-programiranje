using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BibliotekaBack.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Biblioteke",
                columns: table => new
                {
                    BibliotekaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biblioteke", x => x.BibliotekaID);
                });

            migrationBuilder.CreateTable(
                name: "Knjige",
                columns: table => new
                {
                    KnjigaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naslov = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImeAutora = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Izdavac = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GodinaIzdavanja = table.Column<int>(type: "int", nullable: false),
                    EvidencioniBroj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IzdataKnjiga = table.Column<bool>(type: "bit", nullable: false),
                    PripadaBiblioteciBibliotekaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knjige", x => x.KnjigaID);
                    table.ForeignKey(
                        name: "FK_Knjige_Biblioteke_PripadaBiblioteciBibliotekaID",
                        column: x => x.PripadaBiblioteciBibliotekaID,
                        principalTable: "Biblioteke",
                        principalColumn: "BibliotekaID");
                });

            migrationBuilder.CreateTable(
                name: "Izdavanja",
                columns: table => new
                {
                    IzdavanjeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IzdataKnjigaKnjigaID = table.Column<int>(type: "int", nullable: true),
                    IzBibliotekeBibliotekaID = table.Column<int>(type: "int", nullable: true),
                    DatumIzdavanja = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DatumVracanja = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izdavanja", x => x.IzdavanjeID);
                    table.ForeignKey(
                        name: "FK_Izdavanja_Biblioteke_IzBibliotekeBibliotekaID",
                        column: x => x.IzBibliotekeBibliotekaID,
                        principalTable: "Biblioteke",
                        principalColumn: "BibliotekaID");
                    table.ForeignKey(
                        name: "FK_Izdavanja_Knjige_IzdataKnjigaKnjigaID",
                        column: x => x.IzdataKnjigaKnjigaID,
                        principalTable: "Knjige",
                        principalColumn: "KnjigaID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Izdavanja_IzBibliotekeBibliotekaID",
                table: "Izdavanja",
                column: "IzBibliotekeBibliotekaID");

            migrationBuilder.CreateIndex(
                name: "IX_Izdavanja_IzdataKnjigaKnjigaID",
                table: "Izdavanja",
                column: "IzdataKnjigaKnjigaID");

            migrationBuilder.CreateIndex(
                name: "IX_Knjige_PripadaBiblioteciBibliotekaID",
                table: "Knjige",
                column: "PripadaBiblioteciBibliotekaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Izdavanja");

            migrationBuilder.DropTable(
                name: "Knjige");

            migrationBuilder.DropTable(
                name: "Biblioteke");
        }
    }
}
