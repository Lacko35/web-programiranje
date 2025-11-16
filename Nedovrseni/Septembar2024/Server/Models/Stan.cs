using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Stan
    {
        [Key]
        public int BrojStana { get; set; }

        [MaxLength(25)]
        public required string ImeVlasnika { get; set; }

        public required int Kvadratura { get; set; }

        public required int BrojClanova { get; set; }

        public List<Racun>? Racuni { get; set; }

        public Stan()
        {
            Racuni = new List<Racun>();
        }
    }
}