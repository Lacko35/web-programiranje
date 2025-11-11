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
                name: "Grupe",
                columns: table => new
                {
                    GrupaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ImeVaspitaca = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    BojaGrupe = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TrenutniBrojDece = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupe", x => x.GrupaID);
                });

            migrationBuilder.CreateTable(
                name: "Deca",
                columns: table => new
                {
                    DeteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ImeRoditelja = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Jmbg = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    PripadaGrupiGrupaID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deca", x => x.DeteID);
                    table.ForeignKey(
                        name: "FK_Deca_Grupe_PripadaGrupiGrupaID",
                        column: x => x.PripadaGrupiGrupaID,
                        principalTable: "Grupe",
                        principalColumn: "GrupaID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deca_PripadaGrupiGrupaID",
                table: "Deca",
                column: "PripadaGrupiGrupaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Deca");

            migrationBuilder.DropTable(
                name: "Grupe");
        }
    }
}
