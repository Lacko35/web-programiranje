using Microsoft.EntityFrameworkCore;

namespace Silosi.Server.Models
{
    public class SilosiContext : DbContext
    {
        public SilosiContext(DbContextOptions<SilosiContext> options) : base(options)
        {
        }

        public DbSet<Fabrika> Fabrike { get; set; }

        public DbSet<Silos> Silosi { get; set; }
    }
}