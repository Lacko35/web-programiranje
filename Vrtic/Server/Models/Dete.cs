using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Server.Models
{
    public class Dete
    {
        [Key]
        public int DeteID { get; set; }

        [MaxLength(10)]
        public required string Ime { get; set; }

        [MaxLength(20)]
        public required string Prezime { get; set; }

        [MaxLength(10)]
        public required string ImeRoditelja { get; set; }

        [MaxLength(13)]
        public required string Jmbg { get; set; }

        [JsonIgnore]
        public Grupa? PripadaGrupi { get; set; }
    }
}