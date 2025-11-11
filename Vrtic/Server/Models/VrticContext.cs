using Microsoft.EntityFrameworkCore;

namespace Server.Models
{
    public class VrticContext : DbContext
    {
        public VrticContext(DbContextOptions<VrticContext> options) : base(options) { }

        public required DbSet<Dete> Deca { get; set; }

        public required DbSet<Grupa> Grupe { get; set; }
    }
}