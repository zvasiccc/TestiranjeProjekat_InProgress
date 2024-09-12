using Backend.Exceptions;
using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Controllers;
using TestiranjeProjekat.DTOs;
using TestiranjeProjekat.Exceptions;
using TestiranjeProjekat.Models;

namespace Backend.Tests
{
    [TestFixture]
    public class TurnirTest
    {
        private AppDbContext appContext;
        private TurnirController turnirController;
        [OneTimeSetUp]
        public void Setup()
        {
            appContext = GlobalSetup.AppContext;
            turnirController = new TurnirController(appContext);
        }
        //!create
        [Test]
        [TestCase("Turnir Rzana", "Rzana", "12.3.2025.", 16, 3, 500, 3)]
        public async Task createTournament_Success(string naziv, string mestoOdrzavanja, string datumOdrzavanja, int maxBrojTimova, int trenutniBrojTimova, int nagrada, int organizatorId)
        {
            var organizator = await appContext.Organizatori.FindAsync(organizatorId);
            if (organizator == null)
                throw new NonExistingOrganizatorException();
            var noviTurnirDTO = new TurnirDTO
            {
                Naziv = naziv,
                MestoOdrzavanja = mestoOdrzavanja,
                DatumOdrzavanja = datumOdrzavanja,
                MaxBrojTimova = maxBrojTimova,
                TrenutniBrojTimova = trenutniBrojTimova,
                Nagrada = nagrada,
                OrganizatorId = organizatorId
            };
            await turnirController.DodajTurnir(noviTurnirDTO);

            // Assert
            var addedTournament = appContext.Turniri.FirstOrDefault(t => t.Naziv == naziv);
            Assert.IsNotNull(addedTournament);
            Assert.That(addedTournament.MestoOdrzavanja, Is.EqualTo(mestoOdrzavanja));
            Assert.That(addedTournament.DatumOdrzavanja, Is.EqualTo(datumOdrzavanja));
            Assert.That(addedTournament.MaxBrojTimova, Is.EqualTo(maxBrojTimova));
            Assert.That(addedTournament.TrenutniBrojTimova, Is.EqualTo(trenutniBrojTimova));
            Assert.That(addedTournament.Nagrada, Is.EqualTo(nagrada));
            Assert.That(addedTournament.Organizator.Id, Is.EqualTo(organizatorId));
        }
        [Test]
        [TestCase("Turnir Rzana", "Rzana", "12.3.2025.", 16, 3, 500, 99)]
        public async Task CreateTournamentNonExistingOrganizator(string naziv, string mestoOdrzavanja, string datumOdrzavanja, int maxBrojTimova, int trenutniBrojTimova, int nagrada, int organizatorId)
        {
            var organizator = await appContext.Organizatori.FindAsync(organizatorId);
            if (organizator != null)
                throw new ExistingOrganizatorException(); //organizator postoji a za test mora da ne postoji
            var noviTurnirDTO = new TurnirDTO
            {
                Naziv = naziv,
                MestoOdrzavanja = mestoOdrzavanja,
                DatumOdrzavanja = datumOdrzavanja,
                MaxBrojTimova = maxBrojTimova,
                TrenutniBrojTimova = trenutniBrojTimova,
                Nagrada = nagrada,
                OrganizatorId = organizatorId
            };
            Assert.ThrowsAsync<NonExistingOrganizatorException>(async () => await turnirController.DodajTurnir(noviTurnirDTO));
        }
        [Test]

        [TestCase("Turnir Rzana", "Rzana", "", 16, 3, 500, 3)]
        public async Task createTournament_EmptyDatumOdrzavanja_ThrowsEmptyFieldException(string naziv, string mestoOdrzavanja, string datumOdrzavanja, int maxBrojTimova, int trenutniBrojTimova, int nagrada, int organizatorId)
        {
            var organizator = await appContext.Organizatori.FindAsync(organizatorId);
            if (organizator == null)
                throw new NonExistingOrganizatorException();
            var noviTurnirDTO = new TurnirDTO
            {
                Naziv = naziv,
                MestoOdrzavanja = mestoOdrzavanja,
                DatumOdrzavanja = datumOdrzavanja,
                MaxBrojTimova = maxBrojTimova,
                TrenutniBrojTimova = trenutniBrojTimova,
                Nagrada = nagrada,
                OrganizatorId = organizatorId
            };

            Assert.ThrowsAsync<EmptyFieldException>(async () => await turnirController.DodajTurnir(noviTurnirDTO));
        }
        //read
        [Test]
        [TestCase]
        public async Task GetAllTournaments_ReturnsEmptyList_WhenNoTournamentsExist()
        {
            appContext.Turniri.RemoveRange(appContext.Turniri);
            await appContext.SaveChangesAsync();
            var result = await turnirController.VratiSveTurnire();
            Assert.IsInstanceOf<List<Turnir>>(result);
            Assert.That(result.Count, Is.EqualTo(0));
        }
        [Test]
        [TestCase]
        public async Task TaskGetAllTournaments_ReturnsCorrectTournamentDetails()
        {
            var receivedTournaments = await turnirController.VratiSveTurnire();
            var expectedTournaments = await appContext.Turniri.ToListAsync();
            Assert.That(receivedTournaments, Is.EqualTo(expectedTournaments));


        }
        [Test]
        [TestCase(10)]
        public async Task PlayerTournaments_ReturnsEmptyList_WhenPlayerIsNotRegisteredForAnyTournaments(int playerId)
        {

            var result = await turnirController.MojiTurniri_Igrac(playerId);
            Assert.IsEmpty(result);
        }
        [Test]
        [TestCase(3)]
        public async Task OrganizatorTournaments_ReturnsEmptyList_WhenOrganizator_DidntCreateAnyTournamen(int organizatorId)
        {

            var result = await turnirController.MojiTurniri_Organizator(organizatorId);
            Assert.IsEmpty(result);
        }
        [Test]
        [TestCase(11)]
        public async Task PlayerTournaments_ReturnsSingleTournament_WhenPlayerIsRegisteredForOneTournament(int playerId)
        {
            var player = await appContext.Igraci.FindAsync(playerId);
            if (player == null)
                throw new NonExistingPlayerException("nisu zadovoljeni uslovi testa");
            var expectedTournaments = appContext.PrijavaIgracSpoj
                 .Where(pis => pis.IgracId == playerId)
                 .Select(pis => new TurnirDTO
                 {
                     Naziv = pis.Prijava.Turnir.Naziv,
                     DatumOdrzavanja = pis.Prijava.Turnir.DatumOdrzavanja,
                     MestoOdrzavanja = pis.Prijava.Turnir.MestoOdrzavanja,
                     MaxBrojTimova = pis.Prijava.Turnir.MaxBrojTimova,
                     TrenutniBrojTimova = pis.Prijava.Turnir.TrenutniBrojTimova,
                     Nagrada = pis.Prijava.Turnir.Nagrada,
                     OrganizatorId = pis.Prijava.Turnir.Organizator.Id
                 })
                 .ToList();

            // Act
            var result = await turnirController.MojiTurniri_Igrac(playerId);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo(expectedTournaments));
        }
        [Test]
        [TestCase(1)]
        public async Task OrganizatorTournaments_ReturnsSingleTournament_WhenOrganizatorCreatedOneTournament(int organizatorId)
        {
            var organizator = await appContext.Organizatori.FindAsync(organizatorId);
            if (organizator == null)
                throw new NonExistingOrganizatorException("organizator ne postoji i nisu zadovoljeni uslovi testa");
            var expectedTournaments = await appContext.Turniri.Where(t => t.Organizator.Id == organizatorId)
           .Select(t => new TurnirDTO
           {
               Id = t.Id,
               Naziv = t.Naziv,
               DatumOdrzavanja = t.DatumOdrzavanja,
               MestoOdrzavanja = t.MestoOdrzavanja,
               MaxBrojTimova = t.MaxBrojTimova,
               TrenutniBrojTimova = t.TrenutniBrojTimova,
               Nagrada = t.Nagrada,
               OrganizatorId = t.Organizator.Id
           }).ToListAsync();

            // Act
            var result = await turnirController.MojiTurniri_Organizator(organizatorId);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo(expectedTournaments));
        }

        [Test]
        [TestCase(4)]
        public async Task PlayerTournaments_ReturnsTournaments_WhenPlayerIsRegisteredForMultipleTournaments(int playerId)
        {
            var expectedTournaments = appContext.PrijavaIgracSpoj
                .Where(pis => pis.IgracId == playerId)
                .Select(pis => new TurnirDTO
                {
                    Naziv = pis.Prijava.Turnir.Naziv,
                    DatumOdrzavanja = pis.Prijava.Turnir.DatumOdrzavanja,
                    MestoOdrzavanja = pis.Prijava.Turnir.MestoOdrzavanja,
                    MaxBrojTimova = pis.Prijava.Turnir.MaxBrojTimova,
                    TrenutniBrojTimova = pis.Prijava.Turnir.TrenutniBrojTimova,
                    Nagrada = pis.Prijava.Turnir.Nagrada,
                    OrganizatorId = pis.Prijava.Turnir.Organizator.Id
                })
                .ToList();

            // Act
            var result = await turnirController.MojiTurniri_Igrac(playerId);
            Assert.That(result, Is.EqualTo(expectedTournaments));


        }
        [Test]
        [TestCase(5)]
        public async Task OrganizatorTournaments_ReturnsMultipleTournament(int organizatorId)
        {
            var organizator = await appContext.Organizatori.FindAsync(organizatorId);
            if (organizator == null)
                throw new NonExistingOrganizatorException("organizator ne postoji i nisu zadovoljeni uslovi testa");
            var expectedTournaments = await appContext.Turniri.Where(t => t.Organizator.Id == organizatorId)
           .Select(t => new TurnirDTO
           {
               Id = t.Id,
               Naziv = t.Naziv,
               DatumOdrzavanja = t.DatumOdrzavanja,
               MestoOdrzavanja = t.MestoOdrzavanja,
               MaxBrojTimova = t.MaxBrojTimova,
               TrenutniBrojTimova = t.TrenutniBrojTimova,
               Nagrada = t.Nagrada,
               OrganizatorId = t.Organizator.Id
           }).ToListAsync();

            // Act
            var result = await turnirController.MojiTurniri_Organizator(organizatorId);

            Assert.That(result, Is.EqualTo(expectedTournaments));
        }
        //delete
        [Test]
        [TestCase(6)]
        public async Task DeleteTournament_Success(int turnirId)
        {
            var turnir = await appContext.Turniri.FindAsync(turnirId);
            if (turnir == null)
                throw new NonExistingTournamentException("turnir ne postoji i ne moze da se obrise, nisu zadovoljeni uslovi testa");

            await turnirController.ObrisiTurnir(turnirId);
            turnir = await appContext.Turniri.FindAsync(turnirId);
            Assert.Null(turnir);
        }
        [Test]
        [TestCase(99)]
        public void DeleteTournament_NonExistingTournament(int turnirId)
        {
            Assert.ThrowsAsync<NonExistingTournamentException>(async () => await turnirController.ObrisiTurnir(turnirId));
        }

    }
}