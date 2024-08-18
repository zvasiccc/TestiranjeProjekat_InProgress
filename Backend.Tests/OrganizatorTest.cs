using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Controllers;
using NUnit.Framework;
using TestiranjeProjekat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Backend.Tests
{
    [TestFixture]
    public class OrganizatorTest
    {

        private AppDbContext appContext;
        private OrganizatorController organizatorController;

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: "TestDatabase")
               .Options;
            appContext = new AppDbContext(options);
            var initialData = new List<Organizator>{
                new Organizator{Id=1,KorisnickoIme="organizator1",Lozinka="111",Ime="Milos",Prezime="Milosevic"},
                new Organizator{Id=2,KorisnickoIme="organizator2",Lozinka="222",Ime="Stefan",Prezime="Stefanovic"},

            };
            appContext.Organizatori.AddRange(initialData);
            appContext.SaveChanges();
            organizatorController = new OrganizatorController(appContext);
        }
        [Test]
        [TestCase("NekiOrganizator", "123", "Jovan", "Jovanovic")]
        public async Task CreateOrganizator_SuccessfullyAddsAdmin_ReturnSuccessMessage(string korisnickoIme, string lozinka, string ime, string prezime)
        {
            var organizatorController = new OrganizatorController(appContext);
            var newOrganizator = new Organizator
            {
                KorisnickoIme = korisnickoIme,
                Lozinka = lozinka,
                Ime = ime,
                Prezime = prezime
            };
            await organizatorController.registrujOrganizatora(newOrganizator);
            var addedOrganizator = appContext.Organizatori.FirstOrDefault(o => o.KorisnickoIme == korisnickoIme);

            // Proveravamo da li je organizator zaista dodat
            Assert.IsNotNull(addedOrganizator, "Organizator nije dodat u bazu.");
            Assert.AreEqual(korisnickoIme, addedOrganizator.KorisnickoIme);
            Assert.AreEqual(lozinka, addedOrganizator.Lozinka);
            Assert.AreEqual(ime, addedOrganizator.Ime);
            Assert.AreEqual(prezime, addedOrganizator.Prezime);
        }
        [Test]
        [TestCase("organizator1", "111", "Nikola", "Nikolic")]
        public async Task CreateOrganizator_DuplicateUsername_ReturnsErrorMessage(string korisnickoIme, string lozinka, string ime, string prezime)
        {

            var organizatorController = new OrganizatorController(appContext);
            var newOrganizator = new Organizator
            {
                KorisnickoIme = korisnickoIme,
                Lozinka = lozinka,
                Ime = ime,
                Prezime = prezime

            };
            var result = await organizatorController.registrujOrganizatora(newOrganizator);
            // Assert
            Assert.IsInstanceOf<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.AreEqual("korisnicko ime vec postoji", conflictResult.Value);
        }
        [Test]
        [TestCase("organizatorTest", "", "Nenad", "Nenadovic")]
        public async Task CreateOrganizaor_EmptyField_ReturnsErrorMessage(string korisnickoIme, string lozinka, string ime, string prezime)
        {
            var organizatorController = new OrganizatorController(appContext);
            var newOrganizator = new Organizator
            {
                KorisnickoIme = korisnickoIme,
                Lozinka = lozinka,
                Ime = ime,
                Prezime = prezime
            };
            var result = await organizatorController.registrujOrganizatora(newOrganizator);
            Assert.IsInstanceOf<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.AreEqual("uneli ste prazno polje", conflictResult.Value);

        }
        [OneTimeTearDown]
        public void TearDown()
        {
            appContext.Dispose();
        }

    }
}