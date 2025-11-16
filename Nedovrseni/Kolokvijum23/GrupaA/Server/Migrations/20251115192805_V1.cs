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
                name: "Prodavnice",
                columns: table => new
                {
                    ProdavnicaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazivProdavnice = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BrojStolova = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prodavnice", x => x.ProdavnicaID);
                });

            migrationBuilder.CreateTable(
                name: "Sendvici",
                columns: table => new
                {
                    SendvicID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CenaSendvica = table.Column<double>(type: "float", nullable: true),
                    IzProdavniceProdavnicaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sendvici", x => x.SendvicID);
                    table.ForeignKey(
                        name: "FK_Sendvici_Prodavnice_IzProdavniceProdavnicaID",
                        column: x => x.IzProdavniceProdavnicaID,
                        principalTable: "Prodavnice",
                        principalColumn: "ProdavnicaID");
                });

            migrationBuilder.CreateTable(
                name: "Sastojci",
                columns: table => new
                {
                    SastojakID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazivSastojka = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    BojaPrikaza = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InicijalnaDebljina = table.Column<double>(type: "float", nullable: false),
                    InicijalnaSirina = table.Column<double>(type: "float", nullable: false),
                    CenaSastojka = table.Column<double>(type: "float", nullable: false),
                    Kolicina = table.Column<int>(type: "int", nullable: false),
                    DeoSendvicaSendvicID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sastojci", x => x.SastojakID);
                    table.ForeignKey(
                        name: "FK_Sastojci_Sendvici_DeoSendvicaSendvicID",
                        column: x => x.DeoSendvicaSendvicID,
                        principalTable: "Sendvici",
                        principalColumn: "SendvicID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sastojci_DeoSendvicaSendvicID",
                table: "Sastojci",
                column: "DeoSendvicaSendvicID");

            migrationBuilder.CreateIndex(
                name: "IX_Sendvici_IzProdavniceProdavnicaID",
                table: "Sendvici",
                column: "IzProdavniceProdavnicaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sastojci");

            migrationBuilder.DropTable(
                name: "Sendvici");

            migrationBuilder.DropTable(
                name: "Prodavnice");
        }
    }
}
