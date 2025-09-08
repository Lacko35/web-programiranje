using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentCarBack.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Automobili",
                columns: table => new
                {
                    AutomobilID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PredjenaKilometraza = table.Column<int>(type: "int", nullable: false),
                    BrojSedista = table.Column<int>(type: "int", nullable: false),
                    CenaPoDanu = table.Column<int>(type: "int", nullable: false),
                    Godiste = table.Column<int>(type: "int", nullable: false),
                    JelIznajmljen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Automobili", x => x.AutomobilID);
                });

            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    KorisnikID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PunoIme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JMBG = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    BrojVozacke = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.KorisnikID);
                });

            migrationBuilder.CreateTable(
                name: "Iznajmljivanja",
                columns: table => new
                {
                    IznajmljivanjeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojDana = table.Column<int>(type: "int", nullable: false),
                    IznajmljeniAutoAutomobilID = table.Column<int>(type: "int", nullable: true),
                    KorisnikAutaKorisnikID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iznajmljivanja", x => x.IznajmljivanjeID);
                    table.ForeignKey(
                        name: "FK_Iznajmljivanja_Automobili_IznajmljeniAutoAutomobilID",
                        column: x => x.IznajmljeniAutoAutomobilID,
                        principalTable: "Automobili",
                        principalColumn: "AutomobilID");
                    table.ForeignKey(
                        name: "FK_Iznajmljivanja_Korisnici_KorisnikAutaKorisnikID",
                        column: x => x.KorisnikAutaKorisnikID,
                        principalTable: "Korisnici",
                        principalColumn: "KorisnikID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Iznajmljivanja_IznajmljeniAutoAutomobilID",
                table: "Iznajmljivanja",
                column: "IznajmljeniAutoAutomobilID");

            migrationBuilder.CreateIndex(
                name: "IX_Iznajmljivanja_KorisnikAutaKorisnikID",
                table: "Iznajmljivanja",
                column: "KorisnikAutaKorisnikID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Iznajmljivanja");

            migrationBuilder.DropTable(
                name: "Automobili");

            migrationBuilder.DropTable(
                name: "Korisnici");
        }
    }
}
