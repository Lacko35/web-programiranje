using System.Text.Json.Serialization;

namespace BibliotekaBack.Models
{
    public class RentedBook
    {
        [Key]
        public int IzdavanjeID { get; set; }

        [JsonIgnore]
        public Book? IzdataKnjiga { get; set; }

        [JsonIgnore]
        public Library? IzBiblioteke { get; set; }

        public DateTime? DatumIzdavanja { get; set; }

        public DateTime? DatumVracanja { get; set; } = new DateTime(3000, 1, 1);
    }
}