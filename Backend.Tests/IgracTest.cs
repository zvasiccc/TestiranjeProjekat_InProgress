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
            var newPlayer = new Igrac
            {
                KorisnickoIme = korisnickoIme,
                Lozinka = lozinka,
                Ime = ime,
                Prezime = prezime,
                VodjaTima = vodjaTima
            };
            //todo proveri da ne postoji vec taj igrac, pa onda radi ovo
            //ako postoji onda bazim izuzetak u testu, nisu ispunjeni uslovi za odrzavanje testa
            await igracController.RegistrujIgraca(newPlayer);
            var addedPlayer = appContext.Igraci.FirstOrDefault(i => i.KorisnickoIme == korisnickoIme);

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
            //todo provera da li postoji igrac
            var result = await igracController.DohvatiIgraca(korisnickoIme);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Igrac>());
        }
        [Test]
        [TestCase("player123")]
        public async Task getPlayer_NonExistingPlayer(string korisnickoIme)
        {
            //todo provera da li ne postoji igrac
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
        //todo test za empty field, prezime najbolje
        //todo test za update tako da korisnicko ime vec postoji u bazi 
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