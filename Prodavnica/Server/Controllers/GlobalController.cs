using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GlobalController : ControllerBase
    {
        private readonly ProdavnicaContext _context;

        public GlobalController(ProdavnicaContext context)
        {
            _context = context;
        }

        [HttpPost("DodajProdavnicu")]
        public async Task<ActionResult> DodajProdavnicu([FromBody] Prodavnica prodavnica)
        {
            await _context.Prodavnice.AddAsync(prodavnica);

            await _context.SaveChangesAsync();

            return Ok(prodavnica);
        }

        [HttpGet("VratiProdavnice")]
        public async Task<ActionResult> VratiProdavnice()
        {
            var prodavnice = await _context.Prodavnice
            .Include(prodavnice => prodavnice.Proizvodi)
            .ToListAsync();

            return Ok(prodavnice);
        }

        [HttpPost("DodajProizvod/{prodavnicaId}")]
        public async Task<ActionResult> DodajProizvod([FromBody] Proizvod proizvod, [FromRoute] int prodavnicaId)
        {
            var p = await _context.Prodavnice.FindAsync(prodavnicaId);

            if (p == null)
            {
                return NotFound("Prodavnica nije pronadjena.");
            }

            proizvod.Prodavnica = p;
            p.Proizvodi.Add(proizvod);

            await _context.Proizvodi.AddAsync(proizvod);

            await _context.SaveChangesAsync();

            return Ok(proizvod);
        }

        [HttpGet("VratiProizvode/{prodavnicaId}")]
        public async Task<ActionResult> VratiProizvode([FromRoute] int prodavnicaId)
        {
            var proizvodi = await _context.Proizvodi
            .Where(pr => pr.Prodavnica!.ProdavnicaID == prodavnicaId)
            .ToListAsync();

            if (proizvodi.Count == 0)
            {
                return NotFound("Nema proizvoda za zadatu prodavnicu.");
            }

            return Ok(proizvodi);
        }

        [HttpPut("KupiProizvod/{prodavnicaId}/{proizvodId}/{kolicina}")]
        public async Task<ActionResult> KupiProizvod([FromRoute] int prodavnicaId, [FromRoute] int proizvodId, [FromRoute] int kolicina)
        {
            var prodavnica = await _context.Prodavnice.Include(p => p.Proizvodi).FirstOrDefaultAsync(p => p.ProdavnicaID == prodavnicaId);

            if (prodavnica == null)
            {
                return NotFound("Prodavnica nije pronadjena.");
            }

            var proizvod = prodavnica.Proizvodi.FirstOrDefault(pr => pr.ProizvodID == proizvodId);

            if (proizvod == null)
            {
                return NotFound("Proizvod nije pronadjen.");
            }

            if (proizvod.Kolicina < kolicina)
            {
                return BadRequest("Nema dovoljno proizvoda na stanju.");
            }

            if (proizvod.Kolicina == 0)
            {
                prodavnica.Proizvodi.Remove(proizvod);
                _context.Proizvodi.Remove(proizvod);

                await _context.SaveChangesAsync();

                return Ok("Proizvod je rasprodat i trenutno nije dostupan.");
            }

            proizvod.Kolicina -= kolicina;

            await _context.SaveChangesAsync();

            return Ok(proizvod);
        }

        [HttpGet("ProizvodiPoKategoriji/{kategorija}/{prodavnicaID}")]
        public async Task<ActionResult> ProizvodiPoKategoriji([FromRoute] string kategorija, [FromRoute] int prodavnicaID)
        {
            var proizvodi = await _context.Proizvodi
            .Where(pr => pr.Kategorija.ToLower() == kategorija.ToLower() && pr.Prodavnica!.ProdavnicaID == prodavnicaID)
            .ToListAsync();

            if (proizvodi.Count == 0)
            {
                return NotFound("Nema proizvoda u zadatoj kategoriji.");
            }

            return Ok(proizvodi);
        }
    }
}