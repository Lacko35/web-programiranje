using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.DTO;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GlobalController : ControllerBase
    {
        private readonly DataContext _context;

        public GlobalController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("DodajStan")]
        public async Task<ActionResult> DodajStan([FromBody] Stan noviStan)
        {
            try
            {
                if (noviStan == null)
                {
                    return BadRequest("Podaci o stanju ne mogu biti prazni.");
                }

                _context.Stanovi.Add(noviStan);
                await _context.SaveChangesAsync();

                return Ok("Stan je uspješno dodat.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DodajRacun/{stanID}")]
        public async Task<ActionResult> DodajRacun([FromBody] RacunDTO noviRacun, [FromRoute] int stanID)
        {
            try
            {
                var stan = await _context.Stanovi.FindAsync(stanID);

                if (stan == null)
                {
                    return NotFound("Stan sa zadanim ID-jem ne postoji.");
                }

                if (noviRacun == null)
                {
                    return BadRequest("Podaci o računu ne mogu biti prazni.");
                }

                Racun r = new Racun
                {
                    MesecIzdavanja = noviRacun.MesecIzdavanja,
                    JelPlacen = noviRacun.JelPlacen,
                    RacunZaVodu = noviRacun.RacunZaVodu,
                };

                r.ZaStan = stan;

                r.IzracunajRacune();

                stan.Racuni!.Add(r);

                _context.Racuni.Add(r);
                await _context.SaveChangesAsync();

                return Ok("Racun je uspesno dodat.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("VratiBrojeveStanova")]
        public async Task<ActionResult> VratiBrojeveStanova()
        {
            try
            {
                var brojeviStanova = await _context.Stanovi
                .Select(s => s.BrojStana)
                .ToListAsync();

                if (brojeviStanova == null || brojeviStanova.Count == 0)
                {
                    return NotFound("Nema dostupnih stanova.");
                }

                return Ok(brojeviStanova);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("StanInformacije/{stanID}")]
        public async Task<ActionResult> StanInformacije([FromRoute] int stanID)
        {
            try
            {
                var stan = await _context.Stanovi.Include(s => s.Racuni).FirstOrDefaultAsync(s => s.BrojStana == stanID);

                if (stan == null)
                {
                    return NotFound("Stan sa zadanim ID-jem ne postoji.");
                }

                return Ok(stan);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ObracunDugovanja/{stanID}")]
        public async Task<ActionResult> ObracunDugovanja([FromRoute] int stanID)
        {
            try
            {
                var stan = await _context.Stanovi.Include(s => s.Racuni).FirstOrDefaultAsync(s => s.BrojStana == stanID);

                if (stan == null)
                {
                    return NotFound("Stan sa zadanim ID-jem ne postoji.");
                }

                var neisplaceniRacuni = stan.Racuni!.Where(r => !r.JelPlacen).ToList();

                if (neisplaceniRacuni.Count == 0)
                {
                    return Ok("Nema neisplacenih racuna za obracun.");
                }

                double ukupnoDugovanje = 0;

                foreach (var racun in neisplaceniRacuni)
                {
                    ukupnoDugovanje += racun.RacunZaVodu + racun.RacunZaStruju + racun.RacunZaKomunalneUsluge;
                    racun.JelPlacen = true;
                }

                _context.Racuni.UpdateRange(neisplaceniRacuni);
                await _context.SaveChangesAsync();

                return Ok(ukupnoDugovanje);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("ObrisiStan/{stanID}")]
        public async Task<ActionResult> ObrisiStan([FromRoute] int stanID)
        {
            try
            {
                var stan = await _context.Stanovi.Include(s => s.Racuni).FirstOrDefaultAsync(s => s.BrojStana == stanID);

                if (stan == null)
                {
                    return NotFound("Stan sa zadanim ID-jem ne postoji.");
                }

                _context.Racuni.RemoveRange(stan.Racuni!);
                _context.Stanovi.Remove(stan);
                await _context.SaveChangesAsync();

                return Ok("Stan i njegovi računi su uspešno obrisani.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}