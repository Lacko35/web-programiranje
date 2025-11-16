using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Models
{
    public class Sastojak
    {
        [Key]
        public int SastojakID { get; set; }

        [MaxLength(15)]
        public required string NazivSastojka { get; set; }

        public required string BojaPrikaza { get; set; }

        public required double InicijalnaDebljina { get; set; }

        public required double InicijalnaSirina { get; set; }

        public required double CenaSastojka { get; set; }

        public required int Kolicina { get; set; }

        [JsonIgnore]
        public Sendvic? DeoSendvica { get; set; }
    }
}