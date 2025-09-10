using System.Text.Json.Serialization;

namespace BibliotekaBack.Models
{
    public class Book
    {
        [Key]
        public int KnjigaID { get; set; }

        public required string Naslov { get; set; }

        public required string ImeAutora { get; set; }

        public required string Izdavac { get; set; }

        public required int GodinaIzdavanja { get; set; }

        public required string EvidencioniBroj { get; set; }

        public required bool IzdataKnjiga { get; set; }

        [JsonIgnore]
        public Library? PripadaBiblioteci { get; set; }
    }
}