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
            await page.GotoAsync("http://localhost:4200/");
            await page.Locator("body").ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await page.GetByLabel("Korisničko ime:").ClickAsync();
            await page.GetByLabel("Korisničko ime:").FillAsync("proba1");
            await page.GetByLabel("Korisničko ime:").PressAsync("Tab");
            await page.GetByLabel("Lozinka:").FillAsync("proba");
            await page.GetByLabel("Lozinka:").PressAsync("Enter");
            await page.GetByRole(AriaRole.Button, new() { Name = "Pretraga" }).ClickAsync();
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
        public async Task FiltriranjeTurnira_ShouldDisplayTournaments_WhenSearchIsPerformed()
        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/");
            await page.Locator("body").ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await page.GetByLabel("Korisničko ime:").ClickAsync();
            await page.GetByLabel("Korisničko ime:").FillAsync("proba1");
            await page.GetByLabel("Korisničko ime:").PressAsync("Tab");
            await page.GetByLabel("Lozinka:").FillAsync("proba");
            await page.GetByRole(AriaRole.Button, new() { Name = "Prijavi se" }).ClickAsync();

            await page.GetByPlaceholder("Unesite mesto").ClickAsync();
            await page.GetByPlaceholder("Unesite mesto").FillAsync("Rzana");
            await page.GetByPlaceholder("Unesite minimalnu vrednost").ClickAsync();
            await page.GetByPlaceholder("Unesite minimalnu vrednost").FillAsync("100");
            await page.GetByPlaceholder("Unesite maksimalnu vrednost").ClickAsync();
            await page.GetByPlaceholder("Unesite maksimalnu vrednost").FillAsync("2000");

            await page.GetByRole(AriaRole.Button, new() { Name = "Pretraga" }).ClickAsync();
            await page.WaitForSelectorAsync(".container");

            var turnirList = await page.QuerySelectorAllAsync(".container ul li");
            Assert.IsTrue(turnirList.Count > 0, "Nema prikazanih turnira.");

            foreach (var turnir in turnirList)
            {
                var turnirText = await turnir.InnerTextAsync();
                Assert.IsFalse(string.IsNullOrWhiteSpace(turnirText), "Turnir je prazan.");
            }
        }


    }
}
