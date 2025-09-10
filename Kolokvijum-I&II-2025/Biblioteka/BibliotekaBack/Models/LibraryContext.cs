namespace BibliotekaBack.Models
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions options) : base(options) { }

        public required DbSet<Book> Knjige { get; set; }

        public required DbSet<Library> Biblioteke { get; set; }

        public required DbSet<RentedBook> Izdavanja { get; set; }
    }
}