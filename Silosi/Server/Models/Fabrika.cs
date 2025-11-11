using System.ComponentModel.DataAnnotations;

namespace Silosi.Server.Models
{
    public class Fabrika
    {
        [Key]
        public int FabrikaID { get; set; }

        [MaxLength(50)]
        public required string Naziv { get; set; }

        public List<Silos> Silosi { get; set; } = new();
    }
}