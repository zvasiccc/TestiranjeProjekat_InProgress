using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Controllers;
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
            var newPlayer = new Igrac
            {
                KorisnickoIme = korisnickoIme,
                Lozinka = lozinka,
                Ime = ime,
                Prezime = prezime,
                VodjaTima = vodjaTima
            };
            await igracController.RegistrujIgraca(newPlayer);
            var addedPlayer = appContext.Igraci.FirstOrDefault(i => i.KorisnickoIme == korisnickoIme);

            Assert.IsNotNull(addedPlayer);
            Assert.AreEqual(korisnickoIme, addedPlayer.KorisnickoIme);
            Assert.AreEqual(lozinka, addedPlayer.Lozinka);
            Assert.AreEqual(ime, addedPlayer.Ime);
            Assert.AreEqual(prezime, addedPlayer.Prezime);
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

    }
}