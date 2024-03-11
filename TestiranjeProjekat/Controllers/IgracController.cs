using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Data;
using TestiranjeProjekat.DTOs;
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
        [HttpGet("vratiMoguceSaigrace/{igracId}")]
        public async Task<List<Igrac>> VratiMoguceSaigrace(int igracId)
            //todo izmeni parametar kao na git
        {
            var igraci=await _context.Igraci.Where(i=>i.Id!=igracId).ToListAsync();
            if (igraci.Any())
                return igraci;
            return null;

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
       [HttpPut("izmeniPodatkeOIgracu/{igracId}")]
        //todo izmeni parametar kao na git
        public async Task IzmeniPodatkeOIgracu(int igracId,[FromBody]IgracDTO igrac)
        {
            var stariIgrac = await _context.Igraci.FindAsync(igracId);
            if(stariIgrac==null)
            {
                return;
            }
            stariIgrac.Ime = igrac.Ime;
            stariIgrac.Prezime = igrac.Prezime;
            stariIgrac.KorisnickoIme = igrac.KorisnickoIme;
            await _context.SaveChangesAsync();
        }
        [HttpGet("daLiJeIgracPrijavljenNaTurnir/{turnirId}/{igracId}")]
        public async Task<bool> DaLiJeIgracPrijavljenNaTurnir(int turnirId,int igracId)
        {
            var trazenaPrijava = _context.Prijave
                .Include(p => p.Igraci)
                .Include(p => p.Turnir)
                .FirstOrDefault(p => p.Turnir.Id == turnirId && p.Igraci.Any(i => i.Id == igracId));
            return trazenaPrijava != null;
        }
        [HttpGet("vratiIgraceIzIstogTima/{turnirId}/{igracId}")]
        public async Task<List<Igrac>> VratiIgraceIzIstogTima(int turnirId,int igracId)
        {
           
            var saigraci = await _context.PrijavaIgracSpoj
                .Where(pis => pis.IgracId == igracId && pis.Prijava.Turnir.Id == turnirId)
                .Select(pis=>pis.IgracId!=igracId)
                .ToListAsync();
            Console.WriteLine(saigraci);
            return null;
        }
    }
}
