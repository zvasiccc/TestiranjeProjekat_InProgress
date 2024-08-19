using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.DTOs;
using TestiranjeProjekat.Models;

namespace TestiranjeProjekat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizatorController : ControllerBase
    {
        private readonly AppDbContext _context;
        public OrganizatorController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("registrujOrganizatora")]
        public async Task<ActionResult> registrujOrganizatora([FromBody] Organizator organizator)
        {
            //var postojeciOrganizator = await _context.Organizatori.FindAsync(organizator.Id);
            var sviOrganizatori = await _context.Organizatori.ToListAsync();
            //var postojeciOrganizatorId = await _context.Organizatori.FindAsync(organizator.Id);

            var postojeciOrganizator = await _context.Organizatori.FirstOrDefaultAsync(o => o.KorisnickoIme == organizator.KorisnickoIme);
            if (postojeciOrganizator != null)
            {
                Console.WriteLine(postojeciOrganizator.KorisnickoIme);
                return Conflict("korisnicko ime vec postoji");
            }
            if (string.IsNullOrWhiteSpace(organizator.KorisnickoIme)
            || string.IsNullOrWhiteSpace(organizator.Lozinka)
            || string.IsNullOrWhiteSpace(organizator.Ime)
            || string.IsNullOrWhiteSpace(organizator.Prezime))
            {
                return Conflict("uneli ste prazno polje");
            }
            _context.Organizatori.Add(organizator);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("dohvatiOrganizatora/{korisnickoIme}")]
        public async Task<Organizator> dohvatiOrganizatora(string korisnickoIme)
        {

            var organizator = await _context.Organizatori.FirstOrDefaultAsync(o => o.KorisnickoIme == korisnickoIme);
            return organizator;//vraca null ili organizatora
        }
        [HttpGet("daLiJeOrganizatorTurnira/{organizatorId}/{turnirId}")]
        //todo zasto nece async?
        public async Task<bool> daLiJeOrganizatorTurnira(int organizatorId, int turnirId)
        {
            var organizator = _context.Turniri
                .Where(t => t.Id == turnirId)
                .Select(t => t.Organizator)
                .FirstOrDefault();
            return organizator.Id == organizatorId;
        }
        [HttpPut("izmeniPodatkeOOrganizatoru/{organizatorId}")]
        //todo ubaci req
        public async Task<ActionResult> izmeniPodatkeOOrganizatoru(int organizatorId, [FromBody] OrganizatorDTO organizator)


        {
            var postojeciOrganizator = await _context.Organizatori.FindAsync(organizatorId);
            if (postojeciOrganizator == null)
                return Conflict("ne postoji takav organizator");
            if (string.IsNullOrWhiteSpace(organizator.KorisnickoIme)
           || string.IsNullOrWhiteSpace(organizator.Ime)
           || string.IsNullOrWhiteSpace(organizator.Prezime))
            {
                return Conflict("uneli ste prazno polje");
            }
            postojeciOrganizator.Ime = organizator.Ime;
            postojeciOrganizator.Prezime = organizator.Prezime;
            postojeciOrganizator.KorisnickoIme = organizator.KorisnickoIme;
            await _context.SaveChangesAsync();
            return Ok();

        }
        public async Task<ActionResult> obrisiOrganizatora(string korisnickoIme)
        {
            if (string.IsNullOrWhiteSpace(korisnickoIme))
                return Conflict("uneli ste prazno polje");
            var organizator = await _context.Organizatori.FirstOrDefaultAsync(o => o.KorisnickoIme == korisnickoIme);
            if (organizator == null)
                return Conflict("nepostojeci organizator");
            _context.Organizatori.Remove(organizator);
            await _context.SaveChangesAsync();
            return Ok();

        }
    }
}
