global using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Prodavnica
    {
        [Key]
        public int ProdavnicaID { get; set; }

        public required string Naziv { get; set; }

        public required string Adresa { get; set; }

        public required string BrojTelefona { get; set; }

        public List<Proizvod> Proizvodi { get; set; }

        public Prodavnica()
        {
            Proizvodi = new List<Proizvod>();
        }
    }
}