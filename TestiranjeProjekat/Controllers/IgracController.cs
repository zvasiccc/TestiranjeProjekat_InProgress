using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Data;
using TestiranjeProjekat.Models;
using TestiranjeProjekat.Service;

namespace TestiranjeProjekat.Controllers
{
    [ApiController]
    //Route("[controller]")]
    [Route("[controller]")]
    public class IgracController:ControllerBase
    {
       // private readonly IgracService _igracService;
       private readonly ApplicationDbContext _context;
        public IgracController(ApplicationDbContext context)
        {
            //_igracService = igracService;
            _context = context;
        }
       
        [HttpGet("korisnickoIme/{korisnickoIme}")]
        public async Task<List<Igrac>> IgraciSaSlicnimKorisnickimImenom(string korisnickoIme)
        {
            var igraci = await _context.Igraci
                                           .Where(i => i.KorisnickoIme.Contains(korisnickoIme))
                                           .ToListAsync();

            if (igraci.Any())
            {

                return igraci;
            }
            else
            {

                return null;
            }
        }
        [HttpPost("registrujIgraca")]
        public async Task RegistrujIgraca(Igrac igrac)
        {
            _context.Igraci.Add(igrac);
            await _context.SaveChangesAsync();
        }
        [HttpGet("dohvatiIgraca/{korisnickoIme}")]
        public async Task<Igrac> DohvatiIgraca(string korisnickoIme)
        {
           var igrac=await _context.Igraci.FirstOrDefaultAsync(i=>i.KorisnickoIme==korisnickoIme);
            if(igrac==null)
            {
                return null; //op
            }
            return igrac;
        }
       // [HttpPut("izmeniPodatkeOIgracu")]

        [HttpGet("daLiJeIgracPrijavljenNaTurnir/{turnirId}/{igracId}")]
        public async Task<bool> DaLiJeIgracPrijavljenNaTurnir(int turnirId,int igracId)
        {
            var trazenaPrijava = _context.Prijave
                .Include(p => p.Igraci)
                .Include(p => p.Turnir)
                .FirstOrDefault(p => p.Turnir.Id == turnirId && p.Igraci.Any(i => i.Id == igracId));
            return trazenaPrijava != null;
        }
    }
}
