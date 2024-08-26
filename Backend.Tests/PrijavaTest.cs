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
            //?kad se pokrene drugi put samo ovaj test prolazi, zajedno sa svima nece
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
        }
        [Test]
        [TestCase("Tim1", 2, 3, 1, 2, new int[] { 3, 4, 5 }, 222)]
        public async Task createRegistration_InvalidTournamentId(string nazivTima, int brojSlusalica, int brojRacunara, int brojTastatura, int brojMiseva, int[] igraciId, int turnirId)
        {
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
                await prijavaController.dodajPrijavu(prijava);
            });

        }
        [Test]
        [TestCase("Tim1", 2, 3, 1, 2, new int[] { 999, 4, 5 }, 2)]
        public async Task createRegistration_NonExistingPlayer(string nazivTima, int brojSlusalica, int brojRacunara, int brojTastatura, int brojMiseva, int[] igraciId, int turnirId)
        {
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
            Assert.ThrowsAsync<NonExistingPlayerException>(async () =>
            {
                await prijavaController.dodajPrijavu(prijava);
            });
        }
        //!read
        // [Test]
        // [TestCase(2)]
        // public async Task prijaveNaTurniru_ReturnsCorrectPrijave(int turnirId)
        // {
        //     // Arrange
        //     // Fetch existing data from the context
        //     var existingTurnir = await appContext.Turniri
        //         .Include(t => t.Prijave) // Assuming Turnir includes Prijave in the relationship
        //         .FirstOrDefaultAsync(t => t.Id == turnirId);

        //     if (existingTurnir == null)
        //     {
        //         Assert.Fail($"Turnir with id {turnirId} does not exist.");
        //     }

        //     // Act
        //     var result = await prijavaController.prijaveNaTurniru(turnirId);

        //     // Assert
        //     Assert.IsNotNull(result);
        //     Assert.IsInstanceOf<List<Prijava>>(result);

        //     // Fetch the expected prijave from the existing turnir
        //     var expectedPrijave = existingTurnir.Prijave.ToList();

        //     Assert.AreEqual(expectedPrijave.Count, result.Count);

        //     foreach (var prijava in expectedPrijave)
        //     {
        //         var foundPrijava = result.FirstOrDefault(p => p.Id == prijava.Id);
        //         Assert.IsNotNull(foundPrijava);
        //         Assert.AreEqual(prijava.Turnir.Id, foundPrijava.Turnir.Id);
        //         Assert.AreEqual(prijava.Igraci.Count, foundPrijava.Igraci.Count);
        //         foreach (var igrac in prijava.Igraci)
        //         {
        //             var foundIgrac = foundPrijava.Igraci.FirstOrDefault(pi => pi.Igrac.Id == igrac.Igrac.Id);
        //             Assert.IsNotNull(foundIgrac);
        //         }
        //     }
        // }


        //!delete
        [Test]
        [TestCase(2)]
        public async Task deleteRegistration_asOrganizator_Success(int prijavaId)
        {
            //todo iz prve ne prodje, drugi put kad se pokrene samo on hoce
            var existingRegistration = await appContext.Prijave.FindAsync(prijavaId);
            var svePrijave = await appContext.Prijave.ToListAsync();
            Assert.NotNull(existingRegistration);
            await prijavaController.IzbaciTimSaTurnira(prijavaId);
            var nonExistingRegistration = await appContext.Prijave.FindAsync(prijavaId);
            Assert.Null(nonExistingRegistration);
        }
        [Test]
        [TestCase(1, 1)]
        public async Task deleteRegistration_asTeamLeader_Success(int tournamentId, int playerId)
        {
            var svePrijave = await appContext.Prijave.ToListAsync();
            var prijava = await appContext.Prijave.FirstOrDefaultAsync(p => p.Turnir.Id == tournamentId && p.Igraci.Any(i => i.IgracId == playerId));
            var existingRegistration = await appContext.Prijave
                .Include(p => p.Igraci)
                .Include(p => p.Turnir)
                .FirstOrDefaultAsync(p => p.Turnir.Id == tournamentId && p.Igraci.Any(i => i.IgracId == playerId));
            Assert.NotNull(existingRegistration);
            await prijavaController.OdjaviSvojTimSaTurnira(tournamentId, playerId);
            var nonExistingRegistration = await appContext.Prijave
                .Include(p => p.Igraci)
                .Include(p => p.Turnir)
                .FirstOrDefaultAsync(p => p.Turnir.Id == tournamentId && p.Igraci.Any(i => i.IgracId == playerId));
            Assert.Null(nonExistingRegistration);
        }
        [Test]
        [TestCase(99)]
        public async Task deleteRegistration_asOrganizator_NonExistingRegistrationId(int prijavaId)
        {
            Assert.ThrowsAsync<NonExistingRegistrationException>(async () => await prijavaController.IzbaciTimSaTurnira(prijavaId));
        }
        [Test]
        [TestCase(3)]
        public async Task deleteRegistration_asOrganizator_NonExistingTournament(int prijavaId)
        {
            Assert.ThrowsAsync<NonExistingTournamentException>(async () => await prijavaController.IzbaciTimSaTurnira(prijavaId));
        }
        [OneTimeTearDown]
        public void TearDown()
        {
            appContext.Dispose();
        }

    }
}