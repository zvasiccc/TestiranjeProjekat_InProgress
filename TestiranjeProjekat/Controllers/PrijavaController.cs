using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TestiranjeProjekat.Data;
using TestiranjeProjekat.DTOs;
using TestiranjeProjekat.Models;

namespace TestiranjeProjekat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrijavaController : ControllerBase
    {
        ApplicationDbContext _context;
        public PrijavaController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<Prijava>vratiPrijavuPoId(int prijavaId)
        {
            var prijava = await _context.Prijave.FindAsync(prijavaId);
            if (prijava == null)
                return null;
            return prijava;
        }
        [HttpPost("dodajPrijavu")]
        public async Task dodajPrijavu([FromBody] PrijavaDTO prijava)
        {
            var igraci = await _context.Igraci
                .Where(i => prijava.IgraciId.Contains(i.Id))
                .ToListAsync();
            var turnir = await _context.Turniri.FindAsync(prijava.TurnirId);
            Prijava novaPrijava = new Prijava
            {
                NazivTima = prijava.NazivTima,
                PotrebanBrojSlusalica = prijava.PotrebanBrojSlusalica,
                PotrebanBrojRacunara = prijava.PotrebanBrojRacunara,
                PotrebanBrojTastatura = prijava.PotrebanBrojTastatura,
                PotrebanBrojMiseva = prijava.PotrebanBrojMiseva,
                Turnir = turnir
            };
            novaPrijava.Igraci = new List<PrijavaIgracSpoj>();
            foreach (var igrac in igraci)
            {
                novaPrijava.Igraci.Add(new PrijavaIgracSpoj { Igrac = igrac });
            }
            await _context.Prijave.AddAsync(novaPrijava);
            await _context.SaveChangesAsync();
            
        }
        [HttpGet("prijaveNaTurniru/{turnirId}")]
        public async Task<List<Prijava>> prijaveNaTurniru(int turnirId)
        {
            var prijave = await _context.Prijave
                .Where(p => p.Turnir.Id == turnirId)
                .Include(p => p.Turnir)
                .Include(p => p.Igraci)
                .ToListAsync();
            return prijave;
        }
    }
}
