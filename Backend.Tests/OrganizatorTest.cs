using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Controllers;
using NUnit.Framework;
using TestiranjeProjekat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using TestiranjeProjekat.DTOs;

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
        //create
        [Test]
        [TestCase("NekiOrganizator", "123", "Jovan", "Jovanovic")]
        public async Task CreateOrganizator_SuccessfullyAddsOrganizator_ReturnSuccessMessage(string korisnickoIme, string lozinka, string ime, string prezime)
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


            // Assert.IsNotNull(addedOrganizator, "Organizator nije dodat u bazu.");
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
        //read
        [Test]
        [TestCase("organizator1")]
        public async Task getOrganizator_ReturnsSuccess(string korisnickoIme)
        {
            var result = await organizatorController.dohvatiOrganizatora(korisnickoIme);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Organizator>());
        }
        [Test]
        [TestCase("nepostojeciOrganizator")]
        public async Task getOrganizator_NonExistingOrganizator(string korisnickoIme)
        {
            var result = await organizatorController.dohvatiOrganizatora(korisnickoIme);
            Assert.That(result, Is.Null);
        }
        //update
        [Test]
        [TestCase("1", "Organizator1", "Petar", "Petrovic")]
        public async Task updateOrganizatorProfile_Success(int organizatorId, String korisnickoIme, string ime, string prezime)

        {
            var newDataOrganizator = new OrganizatorDTO
            {
                KorisnickoIme = korisnickoIme,
                Ime = ime,
                Prezime = prezime
            };
            await organizatorController.izmeniPodatkeOOrganizatoru(organizatorId, newDataOrganizator);
            var possiblyUpdatedOrganizator = await appContext.Organizatori.FindAsync(organizatorId);
            Assert.AreEqual(korisnickoIme, possiblyUpdatedOrganizator.KorisnickoIme);
            Assert.AreEqual(ime, possiblyUpdatedOrganizator.Ime);
            Assert.AreEqual(prezime, possiblyUpdatedOrganizator.Prezime);
        }
        [Test]
        [TestCase("9999", "Organizator1", "Petar", "Petrovic")]
        public async Task updateOrganizatorProfile_WrongId_returnsError(int organizatorId, String korisnickoIme, string ime, string prezime)

        {
            var newDataOrganizator = new OrganizatorDTO
            {
                KorisnickoIme = korisnickoIme,
                Ime = ime,
                Prezime = prezime
            };
            var result = await organizatorController.izmeniPodatkeOOrganizatoru(organizatorId, newDataOrganizator);
            Assert.IsInstanceOf<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.AreEqual("ne postoji takav organizator", conflictResult.Value);
        }

        // [Test]
        // [TestCase(1, "organizator5", " ", "Nikolic")]
        // public async Task UpdateOrganizaor_EmptyField_ReturnsErrorMessage(int organizatorId, string korisnickoIme, string ime, string prezime)
        // {

        //     var newOrganizator = new OrganizatorDTO
        //     {
        //         KorisnickoIme = korisnickoIme,
        //         Ime = ime,
        //         Prezime = prezime
        //     };
        //     var result = await organizatorController.izmeniPodatkeOOrganizatoru(organizatorId, newOrganizator);
        //     Assert.IsInstanceOf<ConflictObjectResult>(result);
        //     var conflictResult = result as ConflictObjectResult;
        //     Assert.AreEqual("uneli ste prazno polje", conflictResult.Value);

        // }
        //delete
        [Test]
        [TestCase("organizator2")]
        public async Task deleteOrganizator_Succes(string korisnickoIme)
        {
            var result = await organizatorController.obrisiOrganizatora(korisnickoIme);
            Assert.IsInstanceOf<OkResult>(result); //todo da li ovako ili trazim opet organizatora u bazi da proverim?

        }
        [Test]
        [TestCase("")]
        public async Task deleteOrganizator_EmptyField(string korisnickoIme)
        {
            var result = await organizatorController.obrisiOrganizatora(korisnickoIme);
            Assert.IsInstanceOf<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.AreEqual("uneli ste prazno polje", conflictResult.Value);
        }
        [Test]
        [TestCase("Organizator123")]
        public async Task deleteOrganizator_WrongUsername(string korisnickoIme)
        {
            var result = await organizatorController.obrisiOrganizatora(korisnickoIme);
            Assert.IsInstanceOf<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.AreEqual("nepostojeci organizator", conflictResult.Value);
        }
        [OneTimeTearDown]
        public void TearDown()
        {
            appContext.Dispose();
        }

    }
}