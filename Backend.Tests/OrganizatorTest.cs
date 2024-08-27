using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Controllers;
using NUnit.Framework;
using TestiranjeProjekat.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using TestiranjeProjekat.DTOs;
using TestiranjeProjekat.Exceptions;

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

            appContext = GlobalSetup.AppContext;
            organizatorController = new OrganizatorController(appContext);
        }
        //create
        //todo sve sto vazi  za igraca vazi i ovde, iste provere i testovi da dodam
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

            Assert.ThrowsAsync<ExistingOrganizatorException>(async () => await organizatorController.registrujOrganizatora(newOrganizator));

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
            Assert.ThrowsAsync<EmptyFieldException>(async () => await organizatorController.registrujOrganizatora(newOrganizator));

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
            Assert.ThrowsAsync<NonExistingOrganizatorException>(async () => await organizatorController.izmeniPodatkeOOrganizatoru(organizatorId, newDataOrganizator));
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
            await organizatorController.obrisiOrganizatora(korisnickoIme);
            var organizator = await appContext.Organizatori.FirstOrDefaultAsync(o => o.KorisnickoIme == korisnickoIme);
            Assert.Null(organizator);

        }
        [Test]
        [TestCase("")]
        public async Task deleteOrganizator_EmptyField(string korisnickoIme)
        {

            Assert.ThrowsAsync<EmptyFieldException>(async () => await organizatorController.obrisiOrganizatora(korisnickoIme));
        }
        [Test]
        [TestCase("Organizator123")]
        public async Task deleteOrganizator_WrongUsername(string korisnickoIme)
        {

            Assert.ThrowsAsync<NonExistingOrganizatorException>(async () => await organizatorController.obrisiOrganizatora(korisnickoIme));
        }


    }
}