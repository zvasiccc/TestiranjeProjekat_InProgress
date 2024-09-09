using Backend.Exceptions;
using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Controllers;
using TestiranjeProjekat.DTOs;
using TestiranjeProjekat.Exceptions;

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
        [TestCase("Turnir Rzana", "Rzana", "12.3.2025.", 16, 3, 500, 2)]
        public async Task createTournament_Success(string naziv, string mestoOdrzavanja, string datumOdrzavanja, int maxBrojTimova, int trenutniBrojTimova, int nagrada, int organizatorId)
        {
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
            Assert.IsNull(result);
        }
        [Test]
        [TestCase]
        public async Task TaskGetAllTournaments_ReturnsCorrectTournamentDetails()
        {
            var receivedTournaments = await turnirController.VratiSveTurnire();
            var expectedTournaments = await appContext.Turniri.ToListAsync();
            Assert.That(receivedTournaments, Is.EqualTo(expectedTournaments));
            var firstTournament = receivedTournaments.First();
            var expectedFirstTournament = appContext.Turniri.First();
            var lastTournament = receivedTournaments.Last();
            var expectedLastTournament = appContext.Turniri.Last();
            Assert.That(firstTournament, Is.EqualTo(expectedFirstTournament));
            Assert.That(lastTournament, Is.EqualTo(expectedLastTournament));

            //!Assert.That(turniri, Is.SameAs(appContext.Turniri.ToList()));
            //!Assert.That(turniri, Is.SubsetOf(appContext.Turniri));
            // Assert.That(firstTournament.Naziv, Is.EqualTo(expectedFirstTournament.Naziv));
            // Assert.That(firstTournament.MestoOdrzavanja, Is.EqualTo(expectedFirstTournament.MestoOdrzavanja));
            // Assert.That(lastTournament.Naziv, Is.EqualTo(expectedLastTournament.Naziv));
            // Assert.That(lastTournament.MestoOdrzavanja, Is.EqualTo(expectedLastTournament.MestoOdrzavanja));
        }
        [Test]
        [TestCase(10)]
        public async Task PlayerTournaments_ReturnsEmptyList_WhenPlayerIsNotRegisteredForAnyTournaments(int playerId)
        {

            var result = await turnirController.MojiTurniri(playerId);
            Assert.IsEmpty(result);
        }
        [Test]
        [TestCase(11)]
        public async Task PlayerTournaments_ReturnsSingleTournament_WhenPlayerIsRegisteredForOneTournament(int playerId)
        {

            var expectedTournament = appContext.PrijavaIgracSpoj
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
                .FirstOrDefault();

            // Act
            var result = await turnirController.MojiTurniri(playerId);

            if (result.Count != 1)
                throw new Exception("player is registered for more then one tournament");
            Assert.That(result[0].Naziv, Is.EqualTo(expectedTournament.Naziv));
            Assert.That(result[0].DatumOdrzavanja, Is.EqualTo(expectedTournament.DatumOdrzavanja));
            Assert.That(result[0].MestoOdrzavanja, Is.EqualTo(expectedTournament.MestoOdrzavanja));
        }

        [Test]
        [TestCase(1)]
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
            var result = await turnirController.MojiTurniri(playerId);

            // Assert
            Assert.That(result.Count, Is.EqualTo(expectedTournaments.Count));
            for (int i = 0; i < expectedTournaments.Count; i++)
            {
                Assert.That(result[i].Naziv, Is.EqualTo(expectedTournaments[i].Naziv));
                Assert.That(result[i].DatumOdrzavanja, Is.EqualTo(expectedTournaments[i].DatumOdrzavanja));
                Assert.That(result[i].MestoOdrzavanja, Is.EqualTo(expectedTournaments[i].MestoOdrzavanja));
                Assert.That(result[i].MaxBrojTimova, Is.EqualTo(expectedTournaments[i].MaxBrojTimova));
                Assert.That(result[i].TrenutniBrojTimova, Is.EqualTo(expectedTournaments[i].TrenutniBrojTimova));
                Assert.That(result[i].Nagrada, Is.EqualTo(expectedTournaments[i].Nagrada));
                Assert.That(result[i].OrganizatorId, Is.EqualTo(expectedTournaments[i].OrganizatorId));
            }
        }
        //delete

    }
}