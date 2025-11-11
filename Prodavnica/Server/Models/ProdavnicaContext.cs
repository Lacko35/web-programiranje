using Microsoft.EntityFrameworkCore;

namespace Server.Models
{
    public class ProdavnicaContext : DbContext
    {
        public ProdavnicaContext(DbContextOptions<ProdavnicaContext> options) : base(options)
        {
        }

        public DbSet<Prodavnica> Prodavnice { get; set; }
        public DbSet<Proizvod> Proizvodi { get; set; }
    }
}