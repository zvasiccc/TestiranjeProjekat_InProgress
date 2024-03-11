using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Data;
using TestiranjeProjekat.DTOs;
using TestiranjeProjekat.Models;


namespace TestiranjeProjekat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TurnirController:ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TurnirController(ApplicationDbContext context)
        {
               _context = context ;
        }
        [HttpGet("sviTurniri")]
        public async Task<List<Turnir>> VratiSveTurnire() {
           return  await _context.Turniri.ToListAsync();
        }
        [HttpPost("dodajTurnir")]
        public async Task DodajTurnir(TurnirDTO noviTurnirDTO)
        {
            var organizatorTurnira = await _context.Organizatori.FindAsync(noviTurnirDTO.OrganizatorId);
            if (organizatorTurnira==null)
                return;
            Turnir noviTurnir = new Turnir
            {
                Naziv = noviTurnirDTO.Naziv,
                MestoOdrzavanja = noviTurnirDTO.MestoOdrzavanja,
                DatumOdrzavanja = noviTurnirDTO.DatumOdrzavanja,
                MaxBrojTimova=noviTurnirDTO.MaxBrojTimova,
                Nagrada=noviTurnirDTO.Nagrada,
                Organizator = organizatorTurnira
            };
            await _context.Turniri.AddAsync(noviTurnir);
            await _context.SaveChangesAsync();
        }
    }
}
