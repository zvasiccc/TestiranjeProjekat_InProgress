using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Controllers;
using TestiranjeProjekat.DTOs;
using TestiranjeProjekat.Exceptions;
using TestiranjeProjekat.Models;

namespace Backend.Tests
{
    [TestFixture]
    public class PrijavaTest
    {
        AppDbContext appContext;
        PrijavaController prijavaController;
        [OneTimeSetUp]
        public void Setup()
        {
            appContext = GlobalSetup.AppContext;
            prijavaController = new PrijavaController(appContext);
        }
        [Test]
        [TestCase("Tim1", 2, 3, 1, 2, new int[] { 3, 4, 5 }, 3)]
        public async Task CreateRegistration_Success(string nazivTima, int brojSlusalica, int brojRacunara, int brojTastatura, int brojMiseva, int[] igraciId, int turnirId)
        {
            var turnir = await appContext.Turniri.FindAsync(turnirId);
            if (turnir == null)
                throw new NonExistingTournamentException();


            var turnirDTO = new TurnirDTO
            {
                Id = turnir.Id,
                Naziv = turnir.Naziv,
                MaxBrojTimova = turnir.MaxBrojTimova,
                TrenutniBrojTimova = turnir.TrenutniBrojTimova,
                DatumOdrzavanja = turnir.DatumOdrzavanja,
                MestoOdrzavanja = turnir.MestoOdrzavanja,
                Nagrada = turnir.Nagrada,
                OrganizatorId = turnir.Organizator?.Id
            };
            var igraci = appContext.Igraci.Where(p => igraciId.Contains(p.Id)).ToList();
            var igraciDTO = igraci.Select(p =>
                new IgracDTO
                {
                    Id = p.Id,
                    KorisnickoIme = p.KorisnickoIme,
                    Ime = p.Ime,
                    Prezime = p.Prezime,
                    VodjaTima = p.VodjaTima
                }
            ).ToList();
            var prijava = new PrijavaDTO2
            {
                NazivTima = nazivTima,
                PotrebanBrojSlusalica = brojSlusalica,
                PotrebanBrojRacunara = brojRacunara,
                PotrebanBrojTastatura = brojTastatura,
                PotrebanBrojMiseva = brojMiseva,
                Igraci = igraciDTO,
                Turnir = turnirDTO

            };
            var previousNumberOfTeamsInTournament = turnir.TrenutniBrojTimova;
            await prijavaController.dodajPrijavu(prijava);
            var savedRegistration = await appContext.Prijave
            .Include(p => p.Igraci)
            .ThenInclude(pi => pi.Igrac)
            .FirstOrDefaultAsync(p => p.NazivTima == nazivTima && p.Turnir.Id == turnirId);
            Assert.IsNotNull(savedRegistration, "Prijava nije sačuvana u bazi.");
            Assert.That(savedRegistration.PotrebanBrojSlusalica, Is.EqualTo(brojSlusalica), "Broj slusalica se ne poklapa.");
            Assert.That(savedRegistration.PotrebanBrojRacunara, Is.EqualTo(brojRacunara), "Broj racunara se ne poklapa.");
            Assert.That(savedRegistration.PotrebanBrojTastatura, Is.EqualTo(brojTastatura), "Broj tastatura se ne poklapa.");
            Assert.That(savedRegistration.PotrebanBrojMiseva, Is.EqualTo(brojMiseva), "Broj miseva se ne poklapa.");
            Assert.That(savedRegistration.Turnir.Id, Is.EqualTo(turnirId), "Turnir ID se ne poklapa.");
            Assert.That(savedRegistration.Igraci.Count, Is.EqualTo(igraciId.Length), "Broj igraca u prijavi se ne poklapa.");
            for (int i = 0; i < igraciId.Length; i++)
            {
                Assert.IsTrue(savedRegistration.Igraci.Any(pi => pi.Igrac.Id == igraciId[i]), $"Igrac sa ID {igraciId[i]} nije pronađen u prijavi.");
            }
            var tournament2 = await appContext.Turniri.FindAsync(turnirId);
            var currentlyNumberOfTeamsInTournament = turnir.TrenutniBrojTimova;
            Assert.That(previousNumberOfTeamsInTournament + 1, Is.EqualTo(currentlyNumberOfTeamsInTournament));
        }
        [Test]
        [TestCase("Tim1", 2, 3, 1, 2, new int[] { 3, 4, 5 }, 222)]
        public async Task CreateRegistration_InvalidTournamentIdAsync(string nazivTima, int brojSlusalica, int brojRacunara, int brojTastatura, int brojMiseva, int[] igraciId, int turnirId)
        {
            // var turnir = await appContext.Turniri.FindAsync(turnirId);
            // if(turnir==null)
            // throw new NonExistingTournamentException();
            // var TurnirDTO=new TurnirDTO{
            //     Id=turnir.Id,

            // }
            var prijava = new PrijavaDTO
            {
                NazivTima = nazivTima,
                PotrebanBrojSlusalica = brojSlusalica,
                PotrebanBrojRacunara = brojRacunara,
                PotrebanBrojTastatura = brojTastatura,
                PotrebanBrojMiseva = brojMiseva,
                IgraciId = igraciId.ToList(),
                TurnirId = turnirId
            };
            Assert.ThrowsAsync<NonExistingTournamentException>(async () =>
            {
                //todo problem je kako napraviti prijavaDTo2 sa turnirom koji ne postoji, vrv brisanje ovog testa
                //await prijavaController.dodajPrijavu(prijava);
            });

        }
        [Test]
        [TestCase("Tim1", 2, 3, 1, 2, new int[] { 4, 5, 999 }, 2)]
        [TestCase("Tim1", 2, 3, 1, 2, new int[] { 4, 5, 999, 1000 }, 2)]//provera da 2 igraca ne postoje
        public async Task CreateRegistration_NonExistingPlayerAsync(string nazivTima, int brojSlusalica, int brojRacunara, int brojTastatura, int brojMiseva, int[] igraciId, int turnirId)
        {
            //todo provera da taj igrac zaista ne postoji pa onda test nastavak
            var turnir = await appContext.Turniri.FindAsync(turnirId);
            if (turnir == null)
                throw new NonExistingTournamentException();


            var turnirDTO = new TurnirDTO
            {
                Id = turnir.Id,
                Naziv = turnir.Naziv,
                MaxBrojTimova = turnir.MaxBrojTimova,
                TrenutniBrojTimova = turnir.TrenutniBrojTimova,
                DatumOdrzavanja = turnir.DatumOdrzavanja,
                MestoOdrzavanja = turnir.MestoOdrzavanja,
                Nagrada = turnir.Nagrada,
                OrganizatorId = turnir.Organizator?.Id
            };
            var igraci = appContext.Igraci.Where(p => igraciId.Contains(p.Id)).ToList();
            var igraciDTO = igraci.Select(p =>
                new IgracDTO
                {
                    Id = p.Id,
                    KorisnickoIme = p.KorisnickoIme,
                    Ime = p.Ime,
                    Prezime = p.Prezime,
                    VodjaTima = p.VodjaTima
                }
            ).ToList();
            var prijava = new PrijavaDTO2
            {
                NazivTima = nazivTima,
                PotrebanBrojSlusalica = brojSlusalica,
                PotrebanBrojRacunara = brojRacunara,
                PotrebanBrojTastatura = brojTastatura,
                PotrebanBrojMiseva = brojMiseva,
                Igraci = igraciDTO,
                Turnir = turnirDTO

            };
            Assert.ThrowsAsync<NonExistingPlayerException>(async () =>
            {
                await prijavaController.dodajPrijavu(prijava);
            });
        }

        //!read
        [Test]
        [TestCase(2)]
        public async Task AllTournamentRegistrations_ReturnsSuccess(int turnirId)
        {
            var expectedRegistratons = await appContext.Prijave
                .Where(p => p.Turnir.Id == turnirId)
                .Include(p => p.Turnir)
                .Include(p => p.Igraci)
                .ToListAsync();
            var registrations = await prijavaController.prijaveNaTurniru(turnirId);
            // Assert.That(registrations, Is.EqualTo(expectedRegistratons));
        }
        [Test]
        [TestCase(4)]
        public async Task GetRegistration_Success(int registrationId)
        {
            var expectedRegistration = await appContext.Prijave.FindAsync(registrationId) ?? throw new NonExistingRegistrationException($"The test conditions are not met because a registration with id={registrationId} does not exists");
            var receivedRegistration = await prijavaController.vratiPrijavuPoId(registrationId);
            Assert.That(expectedRegistration, Is.EqualTo(receivedRegistration));
        }
        [Test]
        [TestCase(999)]
        public void GetRegistration_NonExistingId(int registrationId)
        {
            Assert.ThrowsAsync<NonExistingRegistrationException>(async () => await prijavaController.vratiPrijavuPoId(registrationId));
        }


        //!delete
        [Test]
        [TestCase(6)]
        public async Task DeleteRegistration_asOrganizator_Success(int registrationId)
        {
            var expectedRegistration = await appContext.Prijave.FindAsync(registrationId)
            ?? throw new NonExistingRegistrationException($"The test conditions are not met because a registration with id={registrationId} does not exists");
            var turnir = expectedRegistration.Turnir
            ?? throw new NonExistingTournamentException();
            if (turnir.Prijave == null) throw new NonExistingRegistrationException();

            var previousNumberOfTeamsInTournament = turnir.TrenutniBrojTimova;
            await prijavaController.IzbaciTimSaTurnira(registrationId);
            var nextNumberOfTeamsInTournament = turnir.TrenutniBrojTimova;
            Assert.That(previousNumberOfTeamsInTournament, Is.EqualTo(nextNumberOfTeamsInTournament + 1));
            var nonExistingRegistration = await appContext.Prijave.FindAsync(registrationId);
            Assert.That(nonExistingRegistration, Is.Null);

            foreach (var prijava in turnir.Prijave)
            {
                Assert.That(prijava, Is.Not.EqualTo(expectedRegistration));
            }
        }

        [Test]
        [TestCase(99)]
        public void DeleteRegistration_asOrganizator_NonExistingRegistrationId(int prijavaId)
        {
            Assert.ThrowsAsync<NonExistingRegistrationException>(async () => await prijavaController.IzbaciTimSaTurnira(prijavaId));
        }
        [Test]
        [TestCase(3)]
        public void DeleteRegistration_asOrganizator_NonExistingTournament(int prijavaId)
        {
            Assert.ThrowsAsync<NonExistingTournamentException>(async () => await prijavaController.IzbaciTimSaTurnira(prijavaId));
        }
        [Test]
        [TestCase(1, 1)]
        public async Task DeleteRegistration_asTeamLeader_Success(int tournamentId, int playerId)
        {
            var registration = await appContext.Prijave
               .Include(p => p.Igraci)
               .Include(p => p.Turnir)
               .FirstOrDefaultAsync(p => p.Turnir.Id == tournamentId && p.Igraci.Any(i => i.IgracId == playerId))
               ?? throw new NonExistingRegistrationException();
            var tournament = registration.Turnir;
            var previousNumberOfTeamsInTournament = tournament.TrenutniBrojTimova;
            //todo broj timova i da li je iz turnira nestala ta prijava
            //todo test kad nema turnir i kad nema taj playerid
            await prijavaController.OdjaviSvojTimSaTurnira(tournamentId, playerId);
            var nextNumberOfTeamsInTournament = tournament.TrenutniBrojTimova;
            Assert.That(previousNumberOfTeamsInTournament, Is.EqualTo(nextNumberOfTeamsInTournament + 1));
            var nonExistingRegistration = await appContext.Prijave
                .Include(p => p.Igraci)
                .Include(p => p.Turnir)
                .FirstOrDefaultAsync(p => p.Turnir.Id == tournamentId && p.Igraci.Any(i => i.IgracId == playerId));
            Assert.That(nonExistingRegistration, Is.Null);
            foreach (var prijava in tournament.Prijave)
            {
                Assert.That(prijava, Is.Not.EqualTo(registration));
            }
        }

    }
}