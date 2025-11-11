using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Silosi.Server.Models
{
    public class Silos
    {
        [Key]
        public int SilosID { get; set; }

        [MaxLength(20)]
        public required string Oznaka { get; set; }

        public required int Kapacitet { get; set; }

        public required int TrenutnoStanje { get; set; }

        [JsonIgnore]
        public Fabrika? Fabrika { get; set; }
    }
}