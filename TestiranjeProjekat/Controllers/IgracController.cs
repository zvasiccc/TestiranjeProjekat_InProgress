using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.DTOs;
using TestiranjeProjekat.Exceptions;
using TestiranjeProjekat.Models;
namespace TestiranjeProjekat.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class IgracController : ControllerBase
    {

        private readonly AppDbContext _context;
        public IgracController(AppDbContext context)
        {

            _context = context;
        }
        [HttpGet("vratiMoguceSaigrace/{igracId}")]
        public async Task<List<IgracDTO>> VratiMoguceSaigrace(int igracId)

        {
            var igraci = await _context.Igraci
                .Where(i => i.Id != igracId)
                .Select(i => new IgracDTO
                {
                    Id = i.Id,
                    KorisnickoIme = i.KorisnickoIme,
                    Ime = i.Ime,
                    Prezime = i.Prezime,
                    VodjaTima = i.VodjaTima
                })
                .ToListAsync();
            return igraci;
        }
        [HttpGet("korisnickoIme/{korisnickoIme}")]
        public async Task<List<Igrac>> IgraciSaSlicnimKorisnickimImenom(string korisnickoIme)
        {
            var igraci = await _context.Igraci
                                           .Where(i => i.KorisnickoIme.Contains(korisnickoIme))
                                           .ToListAsync();

            return igraci;
        }
        [HttpPost("registrujIgraca")]
        public async Task RegistrujIgraca(Igrac igrac)
        {
            if (string.IsNullOrWhiteSpace(igrac.KorisnickoIme)
                || string.IsNullOrWhiteSpace(igrac.Lozinka)
                || string.IsNullOrWhiteSpace(igrac.Ime)
                || string.IsNullOrWhiteSpace(igrac.Prezime))
            {
                throw new EmptyFieldException();
            }
            var postojeciIgrac = await _context.Igraci.FirstOrDefaultAsync(i => i.KorisnickoIme == igrac.KorisnickoIme);
            if (postojeciIgrac != null)
            {
                throw new ExistingPlayerException();
            }
            _context.Igraci.Add(igrac);
            await _context.SaveChangesAsync();

        }
        [Authorize]
        [HttpGet("dohvatiIgraca/{korisnickoIme}")]
        public async Task<Igrac> DohvatiIgraca(string korisnickoIme)
        {
            var igrac = await _context.Igraci.FirstOrDefaultAsync(i => i.KorisnickoIme == korisnickoIme);
            return igrac; //vraca igraca ili null 
        }
        [HttpPut("izmeniPodatkeOIgracu/{igracId}")]

        public async Task IzmeniPodatkeOIgracu(int igracId, [FromBody] IgracDTO igrac)
        {

            var stariIgrac = await _context.Igraci.FindAsync(igracId);
            if (stariIgrac == null)
            {
                throw new NonExistingPlayerException();
            }
            if (string.IsNullOrWhiteSpace(igrac.KorisnickoIme)
         || string.IsNullOrWhiteSpace(igrac.Ime)
         || string.IsNullOrWhiteSpace(igrac.Prezime))
            {
                throw new EmptyFieldException();
            }

            var existingPlayer = await _context.Igraci.Where(p => p.KorisnickoIme == igrac.KorisnickoIme && p.Id != igracId).FirstOrDefaultAsync();

            if (existingPlayer != null) throw new ExistingPlayerException();
            stariIgrac.Ime = igrac.Ime;
            stariIgrac.Prezime = igrac.Prezime;
            stariIgrac.KorisnickoIme = igrac.KorisnickoIme;

            await _context.SaveChangesAsync();
        }
        [HttpGet("daLiJeIgracPrijavljenNaTurnir/{turnirId}/{igracId}")]
        public async Task<bool> DaLiJeIgracPrijavljenNaTurnir(int turnirId, int igracId)
        {
            var trazenaPrijava = await _context.Prijave
                .Include(p => p.Igraci)
                .Include(p => p.Turnir)
                .Where(p => p.Turnir.Id == turnirId && p.Igraci.Any(i => i.Igrac.Id == igracId)).FirstOrDefaultAsync();

            if (trazenaPrijava != null) return true;
            return false;

        }
        [HttpGet("vratiIgraceIzIstogTima/{turnirId}/{igracId}")]

        public async Task<List<IgracDTO>> VratiIgraceIzIstogTima(int turnirId, int igracId)
        {

            var saigraci = await _context.PrijavaIgracSpoj
                 .Where(pis => pis.IgracId == igracId && pis.Prijava.Turnir.Id == turnirId)
                 .SelectMany(pis => pis.Prijava.Igraci)
                 .Where(prijava => prijava.IgracId != igracId)
                 .Select(i => new IgracDTO
                 {
                     KorisnickoIme = i.Igrac.KorisnickoIme,
                     Ime = i.Igrac.Ime,
                     Prezime = i.Igrac.Prezime,
                     VodjaTima = i.Igrac.VodjaTima
                 })
                 .ToListAsync();
            return saigraci;

        }
    }
}
