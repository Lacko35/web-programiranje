using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCarSistem.RentCarBack.Models;

namespace RentCarSistem.RentCarBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GlobalController : ControllerBase
    {
        private readonly RentCarSistemContext context;

        public GlobalController(RentCarSistemContext _context)
        {
            context = _context;
        }

        [HttpPost("DodajAutomobil")]
        public async Task<ActionResult> DodajAutomobil([FromBody] Automobil a)
        {
            try
            {
                await context.Automobili.AddAsync(a);
                await context.SaveChangesAsync();

                return Ok("Uspesno dodavanje auta");
            }
            catch (Exception e)
            {
                return BadRequest("Greska " + e.Message);
            }
        }

        [HttpPost("IznajmiAutomobil")]
        public async Task<ActionResult> IznajmiAutomobil([FromBody] IznajmljivanjeDTO i)
        {
            try
            {
                var auto = await context.Automobili.FindAsync(i.Automobil);

                if (auto == null)
                {
                    return BadRequest("Ne postoji u evidenciji navedeni auto");
                }

                auto.JelIznajmljen = true;

                var korisnik = new Korisnik
                {
                    PunoIme = i.PunoIme,
                    JMBG = i.JMBG,
                    BrojVozacke = i.BrojVozacke
                };

                var iznajmljivanje = new Iznajmljivanje
                {
                    BrojDana = i.BrojDana,
                    KorisnikAuta = korisnik,
                    IznajmljeniAuto = auto
                };

                await context.Iznajmljivanja.AddAsync(iznajmljivanje);
                await context.SaveChangesAsync();

                return Ok("Uspesno iznajmljen auto");
            }
            catch (Exception e)
            {
                return BadRequest("Greska: " + e.Message);
            }
        }

        [HttpPost("FiltrirajPrikaz")]
        public async Task<ActionResult> FiltrirajPrikaz([FromBody] FiltriranjeDTO f)
        {
            try
            {
                var upit = context.Automobili.AsQueryable();

                if (!string.IsNullOrEmpty(f.Model))
                {
                    upit = upit.Where(a => a.Model == f.Model);
                }

                if (f.BrojSedista.HasValue)
                {
                    upit = upit.Where(a => a.BrojSedista == f.BrojSedista);
                }

                if (f.Godiste.HasValue)
                {
                    upit = upit.Where(a => a.Godiste == f.Godiste);
                }

                if (f.Kilometri.HasValue)
                {
                    upit = upit.Where(a => a.PredjenaKilometraza == f.Kilometri);
                }

                var automobili = await upit.ToListAsync();

                return Ok(automobili);
            }
            catch (Exception e)
            {
                return BadRequest("Greska: " + e.Message);
            }
        }

        [HttpGet("VratiAutomobile")]
        public async Task<ActionResult> VratiAutomobile()
        {
            try
            {
                var automobili = await context.Automobili.ToListAsync();

                return Ok(automobili);
            }
            catch (Exception e)
            {
                return BadRequest("Greska " + e.Message);
            }
        }

        [HttpGet("VratiModele")]
        public async Task<ActionResult> VratiModele()
        {
            try
            {
                var modeli = await context.Automobili
                .Select(a => new
                {
                    ModelAuta = a.Model
                })
                .ToListAsync();

                return Ok(modeli);
            }
            catch (Exception e)
            {
                return BadRequest("Greska " + e.Message);
            }
        }

        [HttpGet("VratiIznajmljeneAutomobile")]
        public async Task<ActionResult> VratiIznajmljeneAutomobile()
        {
            try
            {
                var iznajmljeniAutomobili = await context.Iznajmljivanja
                .Include(i => i.KorisnikAuta)
                .Include(i => i.IznajmljeniAuto)
                .Select(i => new
                {
                    ImeKorisnika = i.KorisnikAuta!.PunoIme,
                    ModelAutomobila = i.IznajmljeniAuto!.Model,
                    CenaIznajmljivanja = i.BrojDana * i.IznajmljeniAuto!.CenaPoDanu
                })
                .ToListAsync();

                if (iznajmljeniAutomobili.Count == 0)
                {
                    return BadRequest("Nema iznajmljenih automobila");
                }

                return Ok(iznajmljeniAutomobili);
            }
            catch (Exception e)
            {
                return BadRequest("Greska: " + e.Message);
            }
        }
    }
}