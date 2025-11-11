using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Silosi.Server.Models;

namespace Silosi.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppController : ControllerBase
    {
        private readonly SilosiContext _appContext;

        public AppController(SilosiContext appContext)
        {
            _appContext = appContext;
        }

        [HttpPost("DodajFabriku")]
        public async Task<ActionResult> DodajFabriku([FromBody] Fabrika f)
        {
            try
            {
                if (f == null)
                {
                    return BadRequest("Prosledite fabriku.");
                }

                await _appContext.Fabrike.AddAsync(f);
                await _appContext.SaveChangesAsync();

                return Ok("Fabrika je uspesno dodata.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("VratiFabrike")]
        public async Task<ActionResult> VratiFabrike()
        {
            try
            {
                var fabrike = await _appContext.Fabrike.Include(f => f.Silosi).ToListAsync();

                if (fabrike == null || fabrike.Count == 0)
                {
                    return BadRequest("Nema fabrika u bazi.");
                }

                return Ok(fabrike);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DodajSilos/{fabrikaID}")]
        public async Task<ActionResult> DodajSilos(int fabrikaID, [FromBody] Silos s)
        {
            try
            {
                var fabrika = await _appContext.Fabrike.FindAsync(fabrikaID);

                if (fabrika == null)
                {
                    return BadRequest("Fabrika sa datim ID-jem ne postoji.");
                }

                s.Fabrika = fabrika;
                fabrika.Silosi.Add(s);

                await _appContext.Silosi.AddAsync(s);
                await _appContext.SaveChangesAsync();

                return Ok("Silos je uspesno dodat.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("VratiSilose")]
        public async Task<ActionResult> VratiSilose()
        {
            try
            {
                var silosi = await _appContext.Silosi.ToListAsync();

                if (silosi == null || silosi.Count == 0)
                {
                    return BadRequest("Nema silosa u bazi.");
                }

                return Ok(silosi);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("AzurirajKapacitetSilosa/{silosID}/{novaKolicina}")]
        public async Task<ActionResult> AzurirajKapacitetSilosa(int silosID, int novaKolicina)
        {
            try
            {
                var silos = await _appContext.Silosi.FindAsync(silosID);

                if (silos == null)
                {
                    return BadRequest("Silos sa datim ID-jem ne postoji.");
                }

                if (silos.TrenutnoStanje + novaKolicina > silos.Kapacitet)
                {
                    return BadRequest("Nije moguce dodati toliku kolicinu. Prekoracen je kapacitet silosa.");
                }

                silos.TrenutnoStanje += novaKolicina;

                _appContext.Silosi.Update(silos);
                await _appContext.SaveChangesAsync();

                return Ok("Silos je uspesno azuriran.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}