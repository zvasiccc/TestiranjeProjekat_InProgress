using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Controllers;
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
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
            appContext = new AppDbContext(options);
            var initialData = new List<Igrac>{
                new Igrac{Id=1,KorisnickoIme="nikola1",Lozinka="111",Ime="Nikola",Prezime="Milosevic",VodjaTima=true},
                new Igrac{Id=2,KorisnickoIme="petar2",Lozinka="222",Ime="Nemanja",Prezime="Petrovic",VodjaTima=false},
            };
            appContext.Igraci.AddRange(initialData);
            appContext.SaveChanges();
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
            var result = await igracController.RegistrujIgraca(newPlayer);
            Assert.IsInstanceOf<OkResult>(result);
        }
        [OneTimeTearDown]
        public void TearDown()
        {
            appContext.Dispose();
        }
    }
}