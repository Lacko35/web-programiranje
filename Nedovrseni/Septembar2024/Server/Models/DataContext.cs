using Microsoft.EntityFrameworkCore;

namespace Server.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Stan> Stanovi { get; set; }

        public DbSet<Racun> Racuni { get; set; }
    }
}