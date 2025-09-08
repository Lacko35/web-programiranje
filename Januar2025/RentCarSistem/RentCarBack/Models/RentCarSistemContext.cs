using Microsoft.EntityFrameworkCore;

namespace RentCarSistem.RentCarBack.Models
{
    public class RentCarSistemContext : DbContext
    {
        public RentCarSistemContext(DbContextOptions options) : base(options) { }

        public required DbSet<Automobil> Automobili { get; set; }

        public required DbSet<Korisnik> Korisnici { get; set; }
        
        public required DbSet<Iznajmljivanje> Iznajmljivanja { get; set; }
    }
}