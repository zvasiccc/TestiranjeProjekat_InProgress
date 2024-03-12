﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Data;
using TestiranjeProjekat.DTOs;
using TestiranjeProjekat.Models;

namespace TestiranjeProjekat.Controllers
{
    [ApiController]
    //Route("[controller]")]
    [Route("[controller]")]
    public class IgracController:ControllerBase
    {
       // private readonly IgracService _igracService;
       private readonly ApplicationDbContext _context;
        public IgracController(ApplicationDbContext context)
        {
            //_igracService = igracService;
            _context = context;
        }
        [HttpGet("vratiMoguceSaigrace/{igracId}")]
        public async Task<List<IgracDTO>> VratiMoguceSaigrace(int igracId)
            //todo izmeni parametar kao na git, izbaci id
        {
            var igraci = await _context.Igraci
                .Where(i => i.Id != igracId)
                .Select(i => new IgracDTO
                {
                    KorisnickoIme=i.KorisnickoIme,
                    Ime = i.Ime,
                    Prezime = i.Prezime,
                    VodjaTima =i.VodjaTima
                })
                .ToListAsync();
               return igraci;
        }
        [HttpGet("korisnickoIme/{korisnickoIme}")]
        //todo bez id,dto
        public async Task<List<Igrac>> IgraciSaSlicnimKorisnickimImenom(string korisnickoIme)
        {
            var igraci = await _context.Igraci
                                           .Where(i => i.KorisnickoIme.Contains(korisnickoIme))
                                           .ToListAsync();

            return igraci;
        }
        [HttpPost("registrujIgraca")]
        public async Task RegistrujIgraca(Igrac igrac)
        {
            _context.Igraci.Add(igrac);
            await _context.SaveChangesAsync();
        }
        [HttpGet("dohvatiIgraca/{korisnickoIme}")]
        public async Task<Igrac> DohvatiIgraca(string korisnickoIme)
        {
           var igrac=await _context.Igraci.FirstOrDefaultAsync(i=>i.KorisnickoIme==korisnickoIme);
            return igrac;
        }
       [HttpPut("izmeniPodatkeOIgracu/{igracId}")]
        //todo izmeni parametar kao na git
        public async Task IzmeniPodatkeOIgracu(int igracId,[FromBody]IgracDTO igrac)
        {
            var stariIgrac = await _context.Igraci.FindAsync(igracId);
            if(stariIgrac==null)
            {
                return;
            }
            stariIgrac.Ime = igrac.Ime;
            stariIgrac.Prezime = igrac.Prezime;
            stariIgrac.KorisnickoIme = igrac.KorisnickoIme;
            await _context.SaveChangesAsync();
        }
        [HttpGet("daLiJeIgracPrijavljenNaTurnir/{turnirId}/{igracId}")]
        public async Task<Prijava> DaLiJeIgracPrijavljenNaTurnir(int turnirId,int igracId)
        {
            var trazenaPrijava = _context.Prijave
                .Include(p => p.Igraci)
                .Include(p => p.Turnir)
                .FirstOrDefault(p => p.Turnir.Id == turnirId && p.Igraci.Any(i => i.Id == igracId));
            return trazenaPrijava;
            //todo aleksa, ima 2 idija, i ispituje pogresan 
        }
        [HttpGet("vratiIgraceIzIstogTima/{turnirId}/{igracId}")]
        //todo lose radi
        public async Task<List<IgracDTO>> VratiIgraceIzIstogTima(int turnirId,int igracId)
        {

            var saigraci = await _context.PrijavaIgracSpoj
                 .Where(pis => pis.IgracId == igracId && pis.Prijava.Turnir.Id == turnirId)
                 .SelectMany(pis => pis.Prijava.Igraci)
                 .Where(prijava=> igracId!= igracId)
                 .Select(i=> new IgracDTO
                 {
                     KorisnickoIme = i.Igrac.KorisnickoIme,
                     Ime = i.Igrac.Ime,
                     Prezime = i.Igrac.Prezime,
                     VodjaTima = i.Igrac.VodjaTima
                 })
                 .ToListAsync();
            return saigraci;
        }
    }
}
