using System.Data;
using Microsoft.EntityFrameworkCore;

namespace Server.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Prodavnica> Prodavnice { get; set; }

        public DbSet<Sendvic> Sendvici { get; set; }

        public DbSet<Sastojak> Sastojci { get; set; }
    }
}