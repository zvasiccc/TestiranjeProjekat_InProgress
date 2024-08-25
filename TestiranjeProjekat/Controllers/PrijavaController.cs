using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

using TestiranjeProjekat.DTOs;
using TestiranjeProjekat.Models;

namespace TestiranjeProjekat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrijavaController : ControllerBase
    {
        AppDbContext _context;
        public PrijavaController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public async Task<Prijava> vratiPrijavuPoId(int prijavaId)
        {
            var prijava = await _context.Prijave.FindAsync(prijavaId);
            return prijava;
        }
        [HttpPost("dodajPrijavu")]
        public async Task<Prijava> dodajPrijavu([FromBody] PrijavaDTO prijava)
        {

            var igraci = await _context.Igraci
                .Where(i => prijava.IgraciId.Contains(i.Id))
                .ToListAsync();
            int idTurnira = prijava.TurnirId; //ovo je 1
            var turnir = await _context.Turniri.FindAsync(idTurnira);
            turnir.TrenutniBrojTimova++;

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
            return novaPrijava;
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
        [HttpDelete("izbaciTimSaTurnira/{prijavaId}")]
        public async Task IzbaciTimSaTurnira(int prijavaId)
        {
            var prijava = await _context.Prijave.Where(p => p.Id == prijavaId)
                .Include(p => p.Turnir)
                .Include(p => p.Igraci)
                .FirstOrDefaultAsync();

            if (prijava == null)
                return;
            var turnir = await _context.Turniri.FindAsync(prijava.Turnir.Id);
            if (turnir == null) return;

            turnir.TrenutniBrojTimova--;
            turnir.Prijave.Remove(prijava);

            _context.Prijave.Remove(prijava);
            await _context.SaveChangesAsync();
        }
        [HttpDelete("odjaviSvojTimSaTurnira/{turnirId}/{igracId}")]
        public async Task OdjaviSvojTimSaTurnira(int turnirId, int igracId)
        {
            var prijava = await _context.Prijave
                .Include(p => p.Igraci)
                .Include(p => p.Turnir)
                .FirstOrDefaultAsync(p => p.Turnir.Id == turnirId && p.Igraci.Any(i => i.IgracId == igracId));
            var turnir = await _context.Turniri.FindAsync(turnirId);
            if (turnir == null || prijava == null) return;
            turnir.Prijave.Remove(prijava);
            _context.Prijave.Remove(prijava);
            await _context.SaveChangesAsync();
        }
    }
}
