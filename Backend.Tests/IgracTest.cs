using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Controllers;
using TestiranjeProjekat.DTOs;
using TestiranjeProjekat.Exceptions;
using TestiranjeProjekat.Models;

namespace Backend.Tests
{
    [TestFixture]
    public class IgracTest
    {
        private AppDbContext appContext;
        private IgracController igracController;
        [OneTimeSetUp]
        public void Setup()
        {
            appContext = GlobalSetup.AppContext;
            igracController = new IgracController(appContext);
        }

        [Test]
        [TestCase("jovann", "jovan123", "Jovan", "Jovanovic", false)]
        public async Task createPlayer_Succes(string korisnickoIme, string lozinka, string ime, string prezime, bool vodjaTima)
        {
            var possiblyExistingPlayer = await appContext.Igraci.FirstOrDefaultAsync(i => i.KorisnickoIme == korisnickoIme);
            if (possiblyExistingPlayer != null) throw new ExistingPlayerException("The test conditions are not met because a player with that username already exists");
            var newPlayer = new Igrac
            {
                KorisnickoIme = korisnickoIme,
                Lozinka = lozinka,
                Ime = ime,
                Prezime = prezime,
                VodjaTima = vodjaTima
            };

            await igracController.RegistrujIgraca(newPlayer);
            var addedPlayer = await appContext.Igraci.FirstOrDefaultAsync(i => i.KorisnickoIme == korisnickoIme);

            Assert.IsNotNull(addedPlayer);
            Assert.Multiple(() =>
            {
                Assert.That(addedPlayer.KorisnickoIme, Is.EqualTo(korisnickoIme));
                Assert.That(addedPlayer.Lozinka, Is.EqualTo(lozinka));
                Assert.That(addedPlayer.Ime, Is.EqualTo(ime));
                Assert.That(addedPlayer.Prezime, Is.EqualTo(prezime));
            });
        }
        [Test]
        [TestCase("nikola1", "dzontra123", "nikola", "milosevic", false)]
        public async Task createPlayer_UsernameExists(string korisnickoIme, string lozinka, string ime, string prezime, bool vodjaTima)
        {
            var newPlayer = new Igrac
            {
                KorisnickoIme = korisnickoIme,
                Lozinka = lozinka,
                Ime = ime,
                Prezime = prezime,
                VodjaTima = vodjaTima
            };

            var addedPlayer = appContext.Igraci.FirstOrDefault(i => i.KorisnickoIme == korisnickoIme);
            Assert.ThrowsAsync<ExistingPlayerException>(async () =>
            await igracController.RegistrujIgraca(newPlayer));
        }
        [Test]
        [TestCase("3", "nikola123", "nikola", "", false)]
        public async Task createPlayer_EmptyField_ReturnsError(string korisnickoIme, string lozinka, string ime, string prezime, bool vodjaTima)
        {
            var newPlayer = new Igrac
            {
                KorisnickoIme = korisnickoIme,
                Lozinka = lozinka,
                Ime = ime,
                Prezime = prezime,
                VodjaTima = vodjaTima
            };
            Assert.ThrowsAsync<EmptyFieldException>(async () => await igracController.RegistrujIgraca(newPlayer));
        }
        //read
        [Test]
        [TestCase("nikola1")]
        public async Task getPlayer_ReturnsSuccess(string korisnickoIme)
        {

            var possiblyExistingPlayer = appContext.Igraci.FirstOrDefaultAsync(i => i.KorisnickoIme == korisnickoIme);
            if (possiblyExistingPlayer == null) throw new ExistingPlayerException("The test conditions are not met because a player with that username does not exist");
            var result = await igracController.DohvatiIgraca(korisnickoIme);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Igrac>());
        }
        [Test]
        [TestCase("player999")]
        public async Task getPlayer_NonExistingPlayer_Success(string korisnickoIme)
        {

            var possiblyExistingPlayer = await appContext.Igraci.FirstOrDefaultAsync(i => i.KorisnickoIme == korisnickoIme);
            if (possiblyExistingPlayer != null) throw new ExistingPlayerException("The test conditions are not met because a player with that username exists");
            var result = await igracController.DohvatiIgraca(korisnickoIme);
            Assert.IsNull(result);
        }
        [Test]
        [TestCase("3", "petar123", "petar", "jovanovic")]
        public async Task updatePlayerProfile_Success(int playerId, string korisnickoIme, string ime, string prezime)
        {
            var newPlayer = new IgracDTO
            {
                KorisnickoIme = korisnickoIme,
                Ime = ime,
                Prezime = prezime,
            };
            await igracController.IzmeniPodatkeOIgracu(playerId, newPlayer);
            var possiblyUpdatedPlyer = await appContext.Igraci.FindAsync(playerId);
            Assert.AreEqual(korisnickoIme, possiblyUpdatedPlyer.KorisnickoIme);
            Assert.AreEqual(ime, possiblyUpdatedPlyer.Ime);
            Assert.AreEqual(prezime, possiblyUpdatedPlyer.Prezime);
        }

        [Test]
        [TestCase("3", "petar123", "petar", "")]
        public async Task updatePlayerProfile_EmptyField(int playerId, string korisnickoIme, string ime, string prezime)
        {
            var newPlayer = new IgracDTO
            {
                KorisnickoIme = korisnickoIme,
                Ime = ime,
                Prezime = prezime,
            };
            Assert.ThrowsAsync<EmptyFieldException>(async () => await igracController.IzmeniPodatkeOIgracu(playerId, newPlayer));
        }

        [Test]
        [TestCase("4", "vladimir9", "vladimirr", "vladimirovic")]
        public async Task updatePlayerProfile_ExistingUsername(int playerId, string korisnickoIme, string ime, string prezime)
        {
            var newPlayer = new IgracDTO
            {
                KorisnickoIme = korisnickoIme,
                Ime = ime,
                Prezime = prezime,
            };
            Assert.ThrowsAsync<ExistingPlayerException>(async () => await igracController.IzmeniPodatkeOIgracu(playerId, newPlayer));
        }
        [Test]
        [TestCase("32", "Nemanja123", "Nemanja", "Jovanovic")]
        public async Task updatePlayerProfile_NonExistingId(int playerId, string korisnickoIme, string ime, string prezime)
        {
            var newPlayer = new IgracDTO
            {
                KorisnickoIme = korisnickoIme,
                Ime = ime,
                Prezime = prezime,
            };
            Assert.ThrowsAsync<NonExistingPlayerException>(async () => await igracController.IzmeniPodatkeOIgracu(playerId, newPlayer));
        }

    }
}