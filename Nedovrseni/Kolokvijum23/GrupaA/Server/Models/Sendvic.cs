using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Models
{
    public class Sendvic
    {
        [Key]
        public int SendvicID { get; set; }

        public List<Sastojak>? Sastojci { get; set; }

        public double? CenaSendvica { get; set; }

        [JsonIgnore]
        public Prodavnica? IzProdavnice { get; set; }

        public Sendvic()
        {
            Sastojci = new List<Sastojak>();

            CenaSendvica = 0.0;
        }
    }
}