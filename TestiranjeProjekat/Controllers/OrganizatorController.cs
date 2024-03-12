using Microsoft.AspNetCore.Mvc;
using TestiranjeProjekat.Data;
using TestiranjeProjekat.DTOs;
using TestiranjeProjekat.Models;

namespace TestiranjeProjekat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizatorController:ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public OrganizatorController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost("registrujOrganizatora")]
        public async Task registrujOrganizatora([FromBody] Organizator organizator)
        {
            _context.Organizatori.Add(organizator);
            await _context.SaveChangesAsync();
        }
        [HttpGet("daLiJeOrganizatorTurnira/{organizatorId}/{turnirId}")]
        //todo zasto nece async?
        public async Task<bool> daLiJeOrganizatorTurnira(int organizatorId, int turnirId)
        {
            var organizator =  _context.Turniri
                .Where(t => t.Id == turnirId)
                .Select(t => t.Organizator)
                .FirstOrDefault();
            return organizator.Id == organizatorId;
        }
        [HttpPut("izmeniPodatkeOOrganizatoru/{organizatorId}")]
        //todo ubaci req
        public async Task izmeniPodatkeOOrganizatoru(int organizatorId,[FromBody] OrganizatorDTO organizator)
        {
            var postojeciOrganizator = await _context.Organizatori.FindAsync(organizatorId);
            if (postojeciOrganizator == null)
                throw new Exception("ne postoji takav organizator");
            postojeciOrganizator.Ime = organizator.Ime;
            postojeciOrganizator.Prezime = organizator.Prezime;
            postojeciOrganizator.KorisnickoIme = organizator.KorisnickoIme;
            await _context.SaveChangesAsync();

        }
    }
}
