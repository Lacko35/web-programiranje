using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppController : ControllerBase
    {
        private readonly VrticContext _appContext;

        public AppController(VrticContext appContext)
        {
            _appContext = appContext;
        }

        [HttpPost("DodajGrupu")]
        public async Task<ActionResult> DodajGrupu([FromBody] Grupa g)
        {
            try
            {
                if (g == null)
                {
                    return BadRequest("Prosledite grupu!");
                }

                await _appContext.Grupe.AddAsync(g);
                await _appContext.SaveChangesAsync();

                return Ok("Grupa uspesno dodata!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("VratiGrupe")]
        public async Task<ActionResult> VratiGrupe()
        {
            try
            {
                var grupe = await _appContext.Grupe.Include(g => g.Clanovi).ToListAsync();

                if (grupe == null || grupe.Count == 0)
                {
                    return BadRequest("Nema grupa u bazi podataka");
                }

                return Ok(grupe);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DodajClana")]
        public async Task<ActionResult> DodajClana([FromBody] Dete d)
        {
            try
            {
                if (d == null)
                {
                    return BadRequest("Unesite podatke o detetu!");
                }

                var grupe = await _appContext.Grupe.Include(g => g.Clanovi).ToListAsync();

                if (grupe[0].TrenutniBrojDece == grupe[1].TrenutniBrojDece && grupe[1].TrenutniBrojDece == grupe[2].TrenutniBrojDece)
                {
                    grupe[0].Clanovi!.Add(d);
                    d.PripadaGrupi = grupe[0];
                    grupe[0].TrenutniBrojDece++;
                }
                else
                {
                    var grupa = grupe[0];

                    for (int i = 1; i < grupe.Count; i++)
                    {
                        if (grupa.TrenutniBrojDece > grupe[i].TrenutniBrojDece)
                        {
                            grupa = grupe[i];
                        }
                    }

                    grupa.Clanovi!.Add(d);
                    d.PripadaGrupi = grupa;
                    grupa.TrenutniBrojDece++;
                }

                await _appContext.Deca.AddAsync(d);
                await _appContext.SaveChangesAsync();

                return Ok("Uspesno prikljuceno dete grupi!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}