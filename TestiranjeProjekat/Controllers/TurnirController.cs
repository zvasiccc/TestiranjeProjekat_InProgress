using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Data;
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
               _context = context;
        }
        [HttpGet("sviTurniri")]
        public async Task<List<Turnir>> VratiSveTurnire() {
           return  await _context.Turniri.ToListAsync();
        }
        [HttpPost("dodajTurnir")]
        public async Task DodajTurnir(Turnir turnir)
        {
             _context.Add(turnir);
            await _context.SaveChangesAsync();
        }
    }
}
