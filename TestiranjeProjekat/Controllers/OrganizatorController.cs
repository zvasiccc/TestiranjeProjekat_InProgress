using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.DTOs;
using TestiranjeProjekat.Models;
using TestiranjeProjekat.Exceptions;
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
        public async Task registrujOrganizatora([FromBody] Organizator organizator)
        {

            var postojeciOrganizator = await _context.Organizatori.FirstOrDefaultAsync(o => o.KorisnickoIme == organizator.KorisnickoIme);
            if (postojeciOrganizator != null)
            {

                throw new ExistingOrganizatorException();
            }
            if (string.IsNullOrWhiteSpace(organizator.KorisnickoIme)
            || string.IsNullOrWhiteSpace(organizator.Lozinka)
            || string.IsNullOrWhiteSpace(organizator.Ime)
            || string.IsNullOrWhiteSpace(organizator.Prezime))
            {
                throw new EmptyFieldException();
            }
            _context.Organizatori.Add(organizator);
            await _context.SaveChangesAsync();
            return;
        }
        [HttpGet("dohvatiOrganizatora/{korisnickoIme}")]
        public async Task<Organizator> dohvatiOrganizatora(string korisnickoIme)
        {

            var organizator = await _context.Organizatori.FirstOrDefaultAsync(o => o.KorisnickoIme == korisnickoIme) ?? throw new NonExistingOrganizatorException();
            return organizator;//vraca null ili organizatora
        }
        [HttpGet("daLiJeOrganizatorTurnira/{organizatorId}/{turnirId}")]
        //todo zasto nece async?
        public async Task<bool> daLiJeOrganizatorTurnira(int organizatorId, int turnirId)
        {
            var organizator = _context.Turniri
                .Where(t => t.Id == turnirId)
                .Select(t => t.Organizator)
                .FirstOrDefault() ?? throw new NonExistingOrganizatorException();
            return organizator.Id == organizatorId;
        }
        [HttpPut("izmeniPodatkeOOrganizatoru/{organizatorId}")]
        //todo ubaci req
        public async Task izmeniPodatkeOOrganizatoru(int organizatorId, [FromBody] OrganizatorDTO organizator)
        {
            var postojeciOrganizator = await _context.Organizatori.FindAsync(organizatorId);
            if (postojeciOrganizator == null)
                throw new NonExistingOrganizatorException();
            if (string.IsNullOrWhiteSpace(organizator.KorisnickoIme)
           || string.IsNullOrWhiteSpace(organizator.Ime)
           || string.IsNullOrWhiteSpace(organizator.Prezime))
            {
                throw new EmptyFieldException();
            }
            var existingOrganizator = await _context.Organizatori.FirstOrDefaultAsync(p => p.KorisnickoIme == organizator.KorisnickoIme);
            if (existingOrganizator != null) throw new ExistingOrganizatorException();
            postojeciOrganizator.Ime = organizator.Ime;
            postojeciOrganizator.Prezime = organizator.Prezime;
            postojeciOrganizator.KorisnickoIme = organizator.KorisnickoIme;
            await _context.SaveChangesAsync();

        }
        [HttpDelete]
        public async Task<ActionResult> obrisiOrganizatora(string korisnickoIme)
        {
            if (string.IsNullOrWhiteSpace(korisnickoIme))
                throw new EmptyFieldException();
            var organizator = await _context.Organizatori.FirstOrDefaultAsync(o => o.KorisnickoIme == korisnickoIme);
            if (organizator == null)
                throw new NonExistingOrganizatorException();
            _context.Organizatori.Remove(organizator);
            await _context.SaveChangesAsync();
            return Ok();

        }
    }
}
