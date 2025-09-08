using System.ComponentModel.DataAnnotations;

namespace RentCarSistem.RentCarBack.Models
{
    public class Iznajmljivanje
    {
        [Key]
        public int IznajmljivanjeID { get; set; }

        public required int BrojDana { get; set; }

        public Automobil? IznajmljeniAuto { get; set; }

        public Korisnik? KorisnikAuta { get; set; }
    }
}