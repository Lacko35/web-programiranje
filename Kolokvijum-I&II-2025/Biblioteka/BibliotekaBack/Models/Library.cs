namespace BibliotekaBack.Models
{
    public class Library
    {
        [Key]
        public int BibliotekaID { get; set; }

        public required string Ime { get; set; }

        public required string Adresa { get; set; }

        public required string Email { get; set; }

        public List<Book>? Knjige { get; set; } = new List<Book>();

        public List<RentedBook>? IzdateKnjige { get; set; } = new List<RentedBook>();
    }
}