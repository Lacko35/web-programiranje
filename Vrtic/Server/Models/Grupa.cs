using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Grupa
    {
        [Key]
        public int GrupaID { get; set; }

        [MaxLength(15)]
        public required string Naziv { get; set; }

        [MaxLength(10)]
        public required string ImeVaspitaca { get; set; }

        [MaxLength(15)]
        public required string BojaGrupe { get; set; }

        public List<Dete>? Clanovi { get; set; }

        public required int TrenutniBrojDece { get; set; }

        public Grupa()
        {
            Clanovi = new List<Dete>();
            TrenutniBrojDece = 0;
        }
    }
}