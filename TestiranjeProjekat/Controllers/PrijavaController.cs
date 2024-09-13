using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
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
        public async Task<Prijava> dodajPrijavu([FromBody] PrijavaDTO2 prijava)
        {
            if (prijava.Igraci == null)
            {
                throw new NonExistingPlayerException();
            }

            foreach (var igracDto in prijava.Igraci)
            {
                var igrac = await _context.Igraci.FirstOrDefaultAsync(i => i.Id == igracDto.Id);

                if (igrac == null)
                {

                    throw new NonExistingPlayerException();
                }
            }


            var vodja = prijava.Igraci.FirstOrDefault(p => p.VodjaTima == true);
            if (vodja == null) throw new NonExistingTeamLeaderException();
            var turnir = await _context.Turniri.FirstOrDefaultAsync(p => p.Id == prijava.Turnir.Id);
            if (turnir == null)
                throw new NonExistingTournamentException($"tournament does not exists");

            var novaPrijava = new Prijava
            {
                NazivTima = prijava.NazivTima,
                PotrebanBrojSlusalica = prijava.PotrebanBrojSlusalica,
                PotrebanBrojRacunara = prijava.PotrebanBrojRacunara,
                PotrebanBrojTastatura = prijava.PotrebanBrojTastatura,
                PotrebanBrojMiseva = prijava.PotrebanBrojMiseva,
                Turnir = turnir

            };
            if (turnir.TrenutniBrojTimova + 1 > turnir.MaxBrojTimova)
                throw new FullTournamentCapacityException();
            turnir.TrenutniBrojTimova++;
            await _context.Prijave.AddAsync(novaPrijava);
            await _context.SaveChangesAsync();
            novaPrijava.Igraci = prijava.Igraci.Select(p =>
            {
                var igrac = _context.Igraci.FirstOrDefault(q => q.Id == p.Id);
                var x = new PrijavaIgracSpoj
                {
                    Prijava = novaPrijava,
                    PrijavaId = novaPrijava.Id,
                    Igrac = igrac!,
                    IgracId = igrac!.Id
                };
                return x;
            }).ToList();

            _context.Prijave.Update(novaPrijava);
            await _context.SaveChangesAsync();
            return novaPrijava;
        }


        [HttpGet("prijaveNaTurniru/{turnirId}")]
        public async Task<List<PrijavaDTO2>> prijaveNaTurniru(int turnirId)
        {
            var prijaveDTO = await _context.Prijave
                .Where(p => p.Turnir.Id == turnirId)
                .Select(p => new PrijavaDTO2
                {
                    Id = p.Id,
                    NazivTima = p.NazivTima,
                    PotrebanBrojMiseva = p.PotrebanBrojMiseva,
                    PotrebanBrojRacunara = p.PotrebanBrojRacunara,
                    PotrebanBrojSlusalica = p.PotrebanBrojSlusalica,
                    PotrebanBrojTastatura = p.PotrebanBrojTastatura,
                    Igraci = p.Igraci.Select(i => new IgracDTO
                    {
                        Id = i.Igrac.Id,
                        KorisnickoIme = i.Igrac.KorisnickoIme,
                        Ime = i.Igrac.Ime,
                        Prezime = i.Igrac.Prezime,
                        VodjaTima = i.Igrac.VodjaTima

                    }).ToList(),
                    Turnir = new TurnirDTO
                    {
                        Id = p.Turnir.Id,
                        Naziv = p.Turnir.Naziv,
                        DatumOdrzavanja = p.Turnir.DatumOdrzavanja,
                        MestoOdrzavanja = p.Turnir.MestoOdrzavanja,
                        MaxBrojTimova = p.Turnir.MaxBrojTimova,
                        TrenutniBrojTimova = p.Turnir.TrenutniBrojTimova,
                        Nagrada = p.Turnir.Nagrada
                    }
                })
                .ToListAsync();

            return prijaveDTO;
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
