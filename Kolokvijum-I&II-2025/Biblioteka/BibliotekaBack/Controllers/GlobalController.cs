namespace BibliotekaBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GlobalController : ControllerBase
    {
        private readonly LibraryContext context;

        public GlobalController(LibraryContext _context)
        {
            context = _context;
        }

        [HttpGet("VratiBiblioteke")]
        public async Task<ActionResult> VratiBiblioteke()
        {
            try
            {
                var biblioteke = await context.Biblioteke
                .Include(b => b.Knjige)
                .ToListAsync();

                return Ok(biblioteke);
            }
            catch (Exception e)
            {
                return BadRequest("Greska: " + e.Message);
            }
        }

        [HttpPost("DodajBiblioteku")]
        public async Task<ActionResult> DodajBiblioteku([FromBody] Library l)
        {
            try
            {
                await context.Biblioteke.AddAsync(l);
                await context.SaveChangesAsync();

                return Ok("Uspesno dodata biblioteka u bazu!");
            }
            catch (Exception e)
            {
                return BadRequest("Greska: " + e.Message);
            }
        }

        [HttpPost("DodajKnjigu/{idBiblioteke}")]
        public async Task<ActionResult> DodajKnjigu([FromRoute] int idBiblioteke, [FromBody] Book b)
        {
            try
            {
                var biblioteka = await context.Biblioteke.FindAsync(idBiblioteke);

                if (biblioteka == null)
                {
                    return NotFound("Nije pronadjena data biblioteka");
                }

                var knjiga = new Book
                {
                    Naslov = b.Naslov,
                    ImeAutora = b.ImeAutora,
                    Izdavac = b.Izdavac,
                    GodinaIzdavanja = b.GodinaIzdavanja,
                    EvidencioniBroj = b.EvidencioniBroj,
                    PripadaBiblioteci = biblioteka,
                    IzdataKnjiga = false
                };

                biblioteka.Knjige!.Add(knjiga);

                await context.Knjige.AddAsync(knjiga);
                await context.SaveChangesAsync();

                return Ok("Knjiga dodata u bazu i zeljenu biblioteku!");
            }
            catch (Exception e)
            {
                return BadRequest("Greska: " + e.Message);
            }
        }

        [HttpPost("IzdajKnjigu/{idKnjige}/{idBiblioteke}")]
        public async Task<ActionResult> IzdajKnjigu([FromRoute] int idKnjige, [FromRoute] int idBiblioteke)
        {
            try
            {
                var biblioteka = await context.Biblioteke.FindAsync(idBiblioteke);

                if (biblioteka == null)
                {
                    return NotFound("Biblioteka nije pronadjena!");
                }

                var knjiga = await context.Biblioteke
                .Include(b => b.Knjige)
                .SelectMany(b => b.Knjige!)
                .FirstOrDefaultAsync(k => k.KnjigaID == idKnjige);

                if (knjiga == null)
                {
                    return NotFound("Knjiga nije pronadjena!");
                }

                knjiga.IzdataKnjiga = true;

                var izdavanje = new RentedBook
                {
                    DatumIzdavanja = DateTime.Now,
                    IzdataKnjiga = knjiga,
                    IzBiblioteke = biblioteka
                };

                biblioteka.IzdateKnjige!.Add(izdavanje);

                await context.Izdavanja.AddAsync(izdavanje);
                await context.SaveChangesAsync();

                return Ok("Uspesno izdata knjiga!");
            }
            catch (Exception e)
            {
                return BadRequest("Greska: " + e.Message);
            }
        }

        [HttpPost("VratiKnjigu/{idKnjige}/{idBiblioteke}")]
        public async Task<ActionResult> VratiKnjigu([FromRoute] int idKnjige, [FromRoute] int idBiblioteke)
        {
            try
            {
                var biblioteka = await context.Biblioteke.FindAsync(idBiblioteke);

                if (biblioteka == null)
                {
                    return NotFound("Nije pronadjena biblioteka!");
                }

                var izdavanje = await context.Izdavanja
                .Where(i => i.IzBiblioteke!.BibliotekaID == idBiblioteke && i.IzdataKnjiga!.KnjigaID == idKnjige)
                .FirstOrDefaultAsync();

                if (izdavanje == null)
                {
                    return NotFound("Nije moguce pronaci izdavanje!");
                }

                izdavanje.IzdataKnjiga!.IzdataKnjiga = false;
                izdavanje.DatumVracanja = DateTime.Now;
                biblioteka.IzdateKnjige!.Remove(izdavanje);

                await context.SaveChangesAsync();

                return Ok("Uspesno vracena knjiga!");
            }
            catch (Exception e)
            {
                return BadRequest("Greska: " + e.Message);
            }
        }

        [HttpGet("UkupnoIzdatihKnjiga")]
        public async Task<ActionResult> UkupnoIzdatihKnjiga()
        {
            try
            {
                var lista = await context.Biblioteke
                .Include(b => b.IzdateKnjige)
                .Select(b => new
                {
                    ImeBiblioteke = b.Ime,
                    BrojIzdatihKnjiga = b.IzdateKnjige!.Count
                })
                .ToListAsync();

                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest("Greska: " + e.Message);
            }
        }

        [HttpGet("NajcitanijaKnjiga")]
        public async Task<ActionResult> NajcitanijaKnjiga()
        {
            try
            {
                var najcitanijaKnjiga = await context.Izdavanja
                    .Include(izd => izd.IzdataKnjiga)
                    .GroupBy(izd => izd.IzdataKnjiga)
                    .Select(g => new
                    {
                        Naslov = g.Key!.Naslov,
                        Autor = g.Key!.ImeAutora,
                        BrojIzdavanja = g.Count()
                    })
                    .OrderByDescending(x => x.BrojIzdavanja)
                    .FirstOrDefaultAsync();

                return Ok(najcitanijaKnjiga);
            }
            catch (Exception e)
            {
                return BadRequest("Greska: " + e.Message);
            }
        }

        [HttpGet("VratiKnjige/{bibliotekaID}")]
        public async Task<ActionResult> VratiKnjige([FromRoute] int bibliotekaID)
        {
            try
            {
                var knjige = await context.Biblioteke
                .Include(b => b.Knjige)
                .Where(b => b.BibliotekaID == bibliotekaID)
                .SelectMany(b => b.Knjige!)
                .Select(b => new
                {
                    identifikator = b.KnjigaID,
                    NaslovKnjige = b.Naslov,
                    Autor = b.ImeAutora,
                    Godina = b.GodinaIzdavanja,
                    IzdavacKnjige = b.Izdavac,
                    BrojEvidencije = b.EvidencioniBroj,
                    Izdata = b.IzdataKnjiga
                })
                .ToListAsync();

                return Ok(knjige);
            }
            catch (Exception e)
            {
                return BadRequest("Greska: " + e.Message);
            }
        }
        
        [HttpGet("PretraziKnjigeUBiblioteci/{idBiblioteke}/{parametar}")]
        public async Task<ActionResult> PretraziKnjigeUBiblioteci(
            [FromRoute] int idBiblioteke,
            [FromRoute] string parametar)
        {
            if (string.IsNullOrWhiteSpace(parametar))
                return BadRequest("Unesite parametar za pretragu.");

            parametar = parametar.ToLower();

            var biblioteka = await context.Biblioteke
                .Include(b => b.Knjige)
                .FirstOrDefaultAsync(b => b.BibliotekaID == idBiblioteke);

            if (biblioteka == null)
                return NotFound("Biblioteka nije pronađena.");

            var knjige = biblioteka.Knjige!
            .Where(k =>
                k.ImeAutora.ToLower().Contains(parametar) ||
                k.Naslov.ToLower().Contains(parametar)
            )
            .ToList();

            if (!knjige.Any()) return NotFound("Nema rezultata za zadati parametar u ovoj biblioteci.");

            return Ok(knjige);
        }        
    }
}