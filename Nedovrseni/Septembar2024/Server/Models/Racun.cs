using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Server.Models
{
    public class Racun
    {
        [Key]
        public int RacunID { get; set; }

        public required int MesecIzdavanja { get; set; }

        public required bool JelPlacen { get; set; }

        public required double RacunZaVodu { get; set; }

        public double RacunZaStruju { get; set; }
        [NotMapped]
        private double OsnovicaZaStruju = 150.00;

        public double RacunZaKomunalneUsluge { get; set; }
        [NotMapped]
        private double OsnovicaZaKomunalneUsluge = 100.00;

        [ForeignKey("BrojStana")]
        [JsonIgnore]
        public Stan? ZaStan { get; set; }

        public void IzracunajRacune()
        {
            RacunZaStruju = OsnovicaZaStruju * ZaStan!.BrojClanova;

            RacunZaKomunalneUsluge = OsnovicaZaKomunalneUsluge * ZaStan!.BrojClanova;
        }
    }
}