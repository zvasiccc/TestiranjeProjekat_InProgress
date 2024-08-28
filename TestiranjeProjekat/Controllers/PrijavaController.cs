using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

using TestiranjeProjekat.DTOs;
using TestiranjeProjekat.Exceptions;
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
            if (prijava == null) throw new NonExistingRegistrationException();
            return prijava;
        }
        [HttpPost("dodajPrijavu")]
        public async Task<Prijava> dodajPrijavu([FromBody] PrijavaDTO prijava)
        {
            //todo provera da li je preskocen maks broj timova za turnir
            //todo test za to gde je popunjena knap
            //todo test za uspesno dodavanje kad ima jos 1 mesto
            var igraci = await _context.Igraci
                .Where(i => prijava.IgraciId.Contains(i.Id))
                .ToListAsync();
            if (igraci.Count() != prijava.IgraciId.Count())
                throw new NonExistingPlayerException();
            //int idTurnira = prijava.TurnirId;
            var turnir = await _context.Turniri.FindAsync(prijava.TurnirId);
            if (turnir == null)
                throw new NonExistingTournamentException($"tournament with id={prijava.TurnirId} does not exists");
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

            var prijava = await _context.Prijave.FirstOrDefaultAsync(p => p.Id == prijavaId);

            if (prijava != null)
            {
                await _context.Entry(prijava).Reference(p => p.Turnir).LoadAsync();
            }

            if (prijava == null) throw new NonExistingRegistrationException($"registration with id={prijavaId} does not exists");
            var turnir = prijava.Turnir ?? throw new NonExistingTournamentException($"this tournament does not exists in database");
            turnir.TrenutniBrojTimova--;
            if (turnir.Prijave == null)
                throw new NonExistingRegistrationException();
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
            if (turnir == null)
                throw new NonExistingTournamentException();
            if (prijava == null)
                throw new NonExistingRegistrationException();
            if (turnir.Prijave == null) return;
            turnir.Prijave.Remove(prijava);
            turnir.TrenutniBrojTimova--;
            _context.Prijave.Remove(prijava);
            await _context.SaveChangesAsync();
        }
    }
}
