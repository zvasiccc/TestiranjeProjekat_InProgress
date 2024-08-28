using Backend.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

using TestiranjeProjekat.DTOs;
using TestiranjeProjekat.Exceptions;
using TestiranjeProjekat.Models;


namespace TestiranjeProjekat.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TurnirController : ControllerBase
    {
        private readonly AppDbContext _context;
        public TurnirController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("sviTurniri")]
        public async Task<List<Turnir>> VratiSveTurnire()
        {
            var turniri = await _context.Turniri.ToListAsync();
            if (turniri == null || !turniri.Any())
            {
                return new List<Turnir>();
            }
            return turniri;
        }
        [HttpGet("mojiTurniri/{igracid}")]
        public async Task<List<TurnirDTO>> MojiTurniri(int igracId)
        {
            var turniri = await _context.PrijavaIgracSpoj
                .Where(pis => pis.IgracId == igracId)
                .Select(pis => new TurnirDTO
                {
                    Naziv = pis.Prijava.Turnir.Naziv,
                    DatumOdrzavanja = pis.Prijava.Turnir.DatumOdrzavanja,
                    MestoOdrzavanja = pis.Prijava.Turnir.MestoOdrzavanja,
                    MaxBrojTimova = pis.Prijava.Turnir.MaxBrojTimova,
                    TrenutniBrojTimova = pis.Prijava.Turnir.TrenutniBrojTimova,
                    Nagrada = pis.Prijava.Turnir.Nagrada,
                    OrganizatorId = pis.Prijava.Turnir.Organizator.Id
                })
                .ToListAsync();
            return turniri;
        }
        [HttpPost("dodajTurnir")]
        public async Task DodajTurnir(TurnirDTO noviTurnirDTO)
        {
            var organizatorTurnira = await _context.Organizatori.FindAsync(noviTurnirDTO.OrganizatorId);
            var sviOrganizatori = await _context.Organizatori.ToListAsync();
            if (organizatorTurnira == null)
                throw new NonExistingOrganizatorException();
            if (string.IsNullOrWhiteSpace(noviTurnirDTO.Naziv)
             || string.IsNullOrWhiteSpace(noviTurnirDTO.MestoOdrzavanja)
             || string.IsNullOrWhiteSpace(noviTurnirDTO.DatumOdrzavanja))
                throw new EmptyFieldException();
            Turnir noviTurnir = new Turnir
            {
                Naziv = noviTurnirDTO.Naziv,
                MestoOdrzavanja = noviTurnirDTO.MestoOdrzavanja,
                DatumOdrzavanja = noviTurnirDTO.DatumOdrzavanja,
                MaxBrojTimova = noviTurnirDTO.MaxBrojTimova,
                TrenutniBrojTimova = noviTurnirDTO.TrenutniBrojTimova,
                Nagrada = noviTurnirDTO.Nagrada,
                Organizator = organizatorTurnira
            };
            await _context.Turniri.AddAsync(noviTurnir);
            await _context.SaveChangesAsync();
        }
        [HttpGet("filtrirajTurnire")]
        public List<Turnir> FiltrirajTurnire(
        [FromQuery(Name = "pretragaNaziv")] string? pretragaNaziv,
        [FromQuery(Name = "pretragaMesto")] string? pretragaMesto,
        [FromQuery(Name = "pretragaPocetniDatum")] string? pretragaPocetniDatum,
        [FromQuery(Name = "pretragaKrajnjiDatum")] string? pretragaKrajnjiDatum,
        [FromQuery(Name = "pretragaPocetnaNagrada")] int? pretragaPocetnaNagrada,
        [FromQuery(Name = "pretragaKrajnjaNagrada")] int? pretragaKrajnjaNagrada
    )
        {
            IQueryable<Turnir> query = _context.Turniri;

            if (!string.IsNullOrEmpty(pretragaNaziv))
            {
                query = query.Where(t => t.Naziv.Contains(pretragaNaziv));
            }

            if (!string.IsNullOrEmpty(pretragaMesto))
            {
                query = query.Where(t => t.MestoOdrzavanja == pretragaMesto);
            }

            if (!string.IsNullOrEmpty(pretragaPocetniDatum))
            {
                // Pretvaranje stringa u DateTime objekat
                DateTime pocetniDatum = DateTime.ParseExact(pretragaPocetniDatum, "yyyy-MM-dd", null);

                query = query.Where(t => DateTime.ParseExact(t.DatumOdrzavanja, "yyyy-MM-dd", null) >= pocetniDatum);
            }

            if (!string.IsNullOrEmpty(pretragaKrajnjiDatum))
            {
                // Pretvaranje stringa u DateTime objekat
                DateTime krajnjiDatum = DateTime.ParseExact(pretragaKrajnjiDatum, "yyyy-MM-dd", null);

                query = query.Where(t => DateTime.ParseExact(t.DatumOdrzavanja, "yyyy-MM-dd", null) >= krajnjiDatum);
            }

            if (pretragaPocetnaNagrada > 0)
            {
                query = query.Where(t => t.Nagrada >= pretragaPocetnaNagrada);
            }

            if (pretragaKrajnjaNagrada > 0)
            {
                query = query.Where(t => t.Nagrada <= pretragaKrajnjaNagrada);
            }

            var turniri = query.ToList();

            return turniri;
        }
        [HttpDelete("obrisiTurnir/{turnirId}")]
        public async Task ObrisiTurnir(int turnirId)
        {
            var turnir = await _context.Turniri.FindAsync(turnirId);
            _context.Turniri.Remove(turnir);
            await _context.SaveChangesAsync();
        }
    }
}
