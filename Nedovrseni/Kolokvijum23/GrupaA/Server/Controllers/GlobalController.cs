using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GlobalController : ControllerBase
    {
        private readonly DataContext _context;

        public GlobalController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("DodajProdavnicu")]
        public async Task<ActionResult> DodajProdavnicu([FromBody] Prodavnica p)
        {
            try
            {
                if (p == null)
                {
                    return BadRequest("Morate da unesete podatke o prodavnici");
                }

                await _context.Prodavnice.AddAsync(p);
                await _context.SaveChangesAsync();

                return Ok("Prodavnica uspesno dodata u bazu podataka");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("VratiProdavnicu")]
        public async Task<ActionResult> VratiProdavnicu()
        {
            try
            {
                var prodavnica = await _context.Prodavnice.Include(p => p.Sendvici).FirstOrDefaultAsync();

                if (prodavnica == null)
                {
                    return BadRequest("Prodavnica ne postoji u bazi podataka");
                }

                return Ok(prodavnica);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DodajSendvic/{prodavnicaID}")]
        public async Task<ActionResult> DodajSendvic([FromBody] Sendvic s, [FromRoute] int prodavnicaID)
        {
            try
            {
                if (s == null)
                {
                    return BadRequest("Morate uneti podatke vezane za sendvic");
                }

                var prodavnica = await _context.Prodavnice.FindAsync(prodavnicaID);

                if (prodavnica == null)
                {
                    return BadRequest("Prodavnica ne postoji u bazi podataka");
                }

                if (prodavnica.TrenutnoPopunjeniStolovi < prodavnica.BrojStolova)
                {

                    var sastojak1 = await _context.Sastojci.Where(s => s.NazivSastojka == "hleb").FirstOrDefaultAsync();
                    var sastojak2 = await _context.Sastojci.Where(s => s.NazivSastojka == "hleb").FirstOrDefaultAsync();

                    if (sastojak1 != null && sastojak2 != null)
                    {
                        await _context.Sastojci.AddAsync(sastojak1);
                        await _context.Sastojci.AddAsync(sastojak2);

                        s.Sastojci!.Add(sastojak1);
                        s.Sastojci!.Add(sastojak2);

                        prodavnica.Sendvici!.Add(s);
                        prodavnica.TrenutnoPopunjeniStolovi++;

                        await _context.Sendvici.AddAsync(s);
                        await _context.SaveChangesAsync();
                    }

                    return Ok("Sendvic uspesno dodat!");
                }
                else
                {
                    return BadRequest("Svaki sto trenutno poseduje po sendvic!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("VratiSendvice")]
        public async Task<ActionResult> VratiSendvice()
        {
            try
            {
                var sendvici = await _context.Sendvici.Include(s => s.Sastojci).ToListAsync();

                return Ok(sendvici);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DodajSastojak/{sendvicID}")]
        public async Task<ActionResult> DodajSastojak([FromBody] Sastojak s, [FromRoute] int sendvicID)
        {
            try
            {
                if (s == null)
                {
                    return BadRequest("Morate uneti podatke o sastojku");
                }

                var sendvic = await _context.Sendvici.FindAsync(sendvicID);

                if (sendvic == null)
                {
                    return BadRequest("Ne postoji sendvic sa zadatim id-em");
                }

                sendvic.Sastojci!.Add(s);

                await _context.Sastojci.AddAsync(s);
                await _context.SaveChangesAsync();

                return Ok("Uspesno dodat sastojak!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("VratiSastojke")]
        public async Task<ActionResult> VratiSastojke()
        {
            try
            {
                var sastojci = await _context.Sastojci.Select(s => s.NazivSastojka).ToListAsync();

                if (sastojci == null || sastojci.Count == 0)
                {
                    return BadRequest("Nema sastojaka u bazi podataka");
                }

                return Ok(sastojci);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("NaplatiSendvic/{sendvicID}")]
        public async Task<ActionResult> NaplatiSendvic([FromRoute] int sendvicID)
        {
            try
            {
                var sendvic = await _context.Sendvici.Include(s => s.Sastojci).FirstOrDefaultAsync(p => p.SendvicID == sendvicID);

                if (sendvic == null)
                {
                    return BadRequest("Ne postoji sendvic sa zadatim id-em");
                }

                sendvic.CenaSendvica = sendvic.Sastojci!.Sum(s => s.CenaSastojka);

                foreach (var sastojak in sendvic.Sastojci!)
                {
                    if (sastojak.NazivSastojka != "hleb")
                    {
                        sendvic.Sastojci.Remove(sastojak);
                    }
                }

                _context.Sendvici.Update(sendvic);
                await _context.SaveChangesAsync();

                return Ok(sendvic);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DodajSastojak")]
        public async Task<ActionResult> DodajSastojak([FromBody] Sastojak s)
        {
            try
            {
                if (s == null)
                {
                    return BadRequest("Morate uneti podatke o sastojcima");
                }

                await _context.Sastojci.AddAsync(s);
                await _context.SaveChangesAsync();

                return Ok("Uspesno dodat sastojak");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("ObrisiteSendvice")]
        public async Task<ActionResult> ObrisiSendvice()
        {
            try
            {
                var sendvici = await _context.Sendvici.Include(sendvic => sendvic.Sastojci).ToListAsync();

                foreach (var sendvic in sendvici)
                {
                    _context.Sastojci.RemoveRange(sendvic.Sastojci!);
                }

                _context.Sendvici.RemoveRange(sendvici);

                await _context.SaveChangesAsync();

                return Ok("Obrisani sendvici iz baze!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}