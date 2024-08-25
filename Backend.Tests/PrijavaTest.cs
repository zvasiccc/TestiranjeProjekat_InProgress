using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Controllers;
using TestiranjeProjekat.DTOs;
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
            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
            appContext = new AppDbContext(options);
            var initialData = new List<Prijava>{
                new Prijava{Id=1,NazivTima="Tim1",PotrebanBrojSlusalica=2,PotrebanBrojRacunara=2,PotrebanBrojTastatura=2,PotrebanBrojMiseva=1},
                new Prijava{Id=2,NazivTima="Tim2",PotrebanBrojSlusalica=2,PotrebanBrojRacunara=3,PotrebanBrojTastatura=1,PotrebanBrojMiseva=1},
            };
            appContext.Prijave.AddRange(initialData);
            appContext.SaveChanges();
            prijavaController = new PrijavaController(appContext);
        }
        [Test]
        [TestCase("Tim1", "2", "3", "1", "2")]
        public async Task CreateRegistration_Success(string nazivTima, int brojSlusalica, int brojRacunara, int brojTastatura, int brojMiseva)
        {
            var prijava = new PrijavaDTO
            {
                NazivTima = nazivTima,
                PotrebanBrojSlusalica = brojSlusalica,
                PotrebanBrojRacunara = brojRacunara,
                PotrebanBrojTastatura = brojTastatura,
                PotrebanBrojMiseva = brojMiseva
            };
            var result = await prijavaController.dodajPrijavu(prijava);
        }
        [OneTimeTearDown]
        public void TearDown()
        {
            appContext.Dispose();
        }

    }
}