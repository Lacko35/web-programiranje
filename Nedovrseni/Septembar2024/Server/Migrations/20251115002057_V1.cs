using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stanovi",
                columns: table => new
                {
                    BrojStana = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImeVlasnika = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Kvadratura = table.Column<int>(type: "int", nullable: false),
                    BrojClanova = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stanovi", x => x.BrojStana);
                });

            migrationBuilder.CreateTable(
                name: "Racuni",
                columns: table => new
                {
                    RacunID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MesecIzdavanja = table.Column<int>(type: "int", nullable: false),
                    JelPlacen = table.Column<bool>(type: "bit", nullable: false),
                    RacunZaVodu = table.Column<double>(type: "float", nullable: false),
                    RacunZaStruju = table.Column<double>(type: "float", nullable: false),
                    RacunZaKomunalneUsluge = table.Column<double>(type: "float", nullable: false),
                    BrojStana = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Racuni", x => x.RacunID);
                    table.ForeignKey(
                        name: "FK_Racuni_Stanovi_BrojStana",
                        column: x => x.BrojStana,
                        principalTable: "Stanovi",
                        principalColumn: "BrojStana");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Racuni_BrojStana",
                table: "Racuni",
                column: "BrojStana");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Racuni");

            migrationBuilder.DropTable(
                name: "Stanovi");
        }
    }
}
