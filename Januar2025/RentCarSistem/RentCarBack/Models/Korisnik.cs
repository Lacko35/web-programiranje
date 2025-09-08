using System.ComponentModel.DataAnnotations;

namespace RentCarSistem.RentCarBack.Models
{
    public class Korisnik
    {
        [Key]

        public int KorisnikID { get; set; }

        public required string PunoIme { get; set; }

        [MaxLength(13)]
        public required string JMBG { get; set; }

        [MaxLength(9)]
        public required string BrojVozacke { get; set; }
    }
}