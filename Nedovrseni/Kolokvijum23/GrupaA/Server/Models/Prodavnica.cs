using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Prodavnica
    {
        [Key]
        public int ProdavnicaID { get; set; }

        [MaxLength(20)]
        public required string NazivProdavnice { get; set; }

        public required int BrojStolova { get; set; }

        public required int TrenutnoPopunjeniStolovi { get; set; }

        public List<Sendvic>? Sendvici { get; set; }

        public Prodavnica()
        {
            Sendvici = new List<Sendvic>();

            TrenutnoPopunjeniStolovi = 0;
        }
    }
}