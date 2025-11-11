using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Models
{
    public class Proizvod
    {
        [Key]
        public int ProizvodID { get; set; }

        public required string Naziv { get; set; }

        public required string Kategorija { get; set; }

        public required double Cena { get; set; }

        public required int Kolicina { get; set; }

        [JsonIgnore]
        public Prodavnica? Prodavnica { get; set; }
    }
}