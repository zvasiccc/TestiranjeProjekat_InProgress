using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightTests
{
    public class TurnirTest
    {
        private IBrowser _browser;

        [SetUp]
        public async Task Setup()
        {
            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
        }

        [TearDown]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
        }

        [Test]
        public async Task MojiTurniri_ShouldDisplayTournaments_WhenClicked()
        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/login");

            // Unos korisničkog imena i lozinke
            await page.FillAsync("#korisnickoIme", "proba1"); // Zameni sa stvarnim ID-om
            await page.FillAsync("#lozinka", "proba"); // Zameni sa stvarnim ID-om

            // Klik na dugme za prijavu
            await page.ClickAsync("#loginButton"); // Zameni sa stvarnim selektorom za dugme prijave

            // Provera da li je korisnik uspešno preusmeren na početnu stranicu
            await page.WaitForURLAsync("http://localhost:4200/");
            var currentUrl = page.Url;
            Assert.AreEqual("http://localhost:4200/", currentUrl);

            // Klik na link "Moji turniri"
            await page.ClickAsync("a[routerLink='mojiTurniri']");

            // Provera da li se prikazuje stranica sa turnirima
            await page.WaitForSelectorAsync(".container"); // Očekuje se da se prikazuju turniri

            // Proveri da li se prikazuju neki turniri
            var turnirList = await page.QuerySelectorAllAsync(".container ul li");
            Assert.IsTrue(turnirList.Count > 0, "Nema prikazanih turnira.");

            // Ako je potrebno, proveri specifične podatke o turnirima
            foreach (var turnir in turnirList)
            {
                var turnirText = await turnir.InnerTextAsync();
                Assert.IsFalse(string.IsNullOrWhiteSpace(turnirText), "Turnir je prazan.");
            }
        }
        [Test]
        public async Task Pretraga_ShouldDisplayTournaments_WhenSearchIsPerformed()
        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/login");

            // Unos korisničkog imena i lozinke
            await page.FillAsync("#korisnickoIme", "proba1");
            await page.FillAsync("#lozinka", "proba");

            // Klik na dugme za prijavu
            await page.ClickAsync("#loginButton");

            // Provera da li je korisnik uspešno preusmeren na početnu stranicu
            await page.WaitForURLAsync("http://localhost:4200/");
            var currentUrl = page.Url;
            Assert.AreEqual("http://localhost:4200/", currentUrl);


            // Sačekaj da se komponenta za pretragu učita
            //await page.WaitForSelectorAsync("#naziv");

            // Unesi podatke za pretragu
            // await page.FillAsync("#naziv", "Turnir u Rzani");
            // await page.FillAsync("#mesto", "Rzana");
            // await page.FillAsync("#pocetniDatum", "2024-01-01");
            // await page.FillAsync("#krajnjiDatum", "2024-10-10");
            //await page.FillAsync("#pocetnaNagrada", "100");
            // await page.FillAsync("#krajnjaNagrada", "2000");

            // Klik na dugme za pretragu
            await page.ClickAsync("#pretragaButton");

            // Provera da li se prikazuju rezultati pretrage
            await page.WaitForSelectorAsync(".container"); // Očekuje se da se prikazuju turniri

            // Proveri da li se prikazuju neki turniri
            var turnirList = await page.QuerySelectorAllAsync(".container ul li");
            Assert.IsTrue(turnirList.Count > 0, "Nema prikazanih turnira.");

            // Ako je potrebno, proveri specifične podatke o turnirima
            foreach (var turnir in turnirList)
            {
                var turnirText = await turnir.InnerTextAsync();
                Assert.IsFalse(string.IsNullOrWhiteSpace(turnirText), "Turnir je prazan.");
            }
        }

    }
}
