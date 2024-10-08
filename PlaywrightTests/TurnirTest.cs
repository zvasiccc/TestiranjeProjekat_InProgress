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
        public async Task MojiTurniri_Kao_Igrac()
        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/login");

            await page.FillAsync("#korisnickoIme", "proba1");
            await page.FillAsync("#lozinka", "proba");

            await page.ClickAsync("#loginButton");

            await page.GetByRole(AriaRole.Link, new() { Name = "Moji turniri" }).ClickAsync();
            await page.WaitForSelectorAsync(".container");

            var turnirList = await page.QuerySelectorAllAsync(".container ul li");
            Assert.IsTrue(turnirList.Count > 0, "Nema prikazanih turnira.");
            foreach (var turnir in turnirList)
            {
                var turnirText = await turnir.InnerTextAsync();
                Assert.IsFalse(string.IsNullOrWhiteSpace(turnirText), "Turnir je prazan.");
            }
            await page.ScreenshotAsync(new() { Path = "../../../Slike/TurniriIgraca.png" });
        }
        [Test]
        public async Task MojiTurniri_Kao_Organizator()
        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/login");

            await page.FillAsync("#korisnickoIme", "dzontra");
            await page.FillAsync("#lozinka", "dzontric");
            await page.ClickAsync("#loginButton");

            await page.ClickAsync("a[routerLink='mojiTurniri']");
            await page.WaitForSelectorAsync(".container");

            var turnirList = await page.QuerySelectorAllAsync(".container ul li");
            Assert.IsTrue(turnirList.Count > 0, "Nema prikazanih turnira.");

            foreach (var turnir in turnirList)
            {
                var turnirText = await turnir.InnerTextAsync();
                Assert.IsFalse(string.IsNullOrWhiteSpace(turnirText), "Turnir je prazan.");

                var prijavljeniTimoviButton = await turnir.QuerySelectorAsync("button:has-text('Prijavljeni timovi')");
                var obrisiTurnirButton = await turnir.QuerySelectorAsync("button:has-text('Obrisi turnir')");

                Assert.NotNull(prijavljeniTimoviButton);
                Assert.NotNull(obrisiTurnirButton);
            }
            await page.ScreenshotAsync(new() { Path = "../../../Slike/TurniriOrganizatora.png" });
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
            await page.WaitForSelectorAsync(".container");

            var turnirList = await page.QuerySelectorAllAsync(".container ul li");
            Assert.IsTrue(turnirList.Count > 0, "Nema prikazanih turnira.");
            foreach (var turnir in turnirList)
            {
                var turnirText = await turnir.InnerTextAsync();
                Assert.IsFalse(string.IsNullOrWhiteSpace(turnirText), "Turnir je prazan.");
            }
            await page.ScreenshotAsync(new() { Path = "../../../Slike/PretragaTurnira.png" });
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
            await page.ScreenshotAsync(new() { Path = "../../../Slike/FiltriranjeTurnira.png" });
        }
        [Test]
        public async Task KreirajTurnir()
        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/login");
            await page.GetByLabel("Korisničko ime:").ClickAsync();
            await page.GetByLabel("Korisničko ime:").FillAsync("dzontra");
            await page.GetByLabel("Korisničko ime:").PressAsync("Tab");
            await page.GetByLabel("Lozinka:").FillAsync("dzontric");
            await page.GetByLabel("Lozinka:").PressAsync("Enter");
            await page.GetByRole(AriaRole.Button, new() { Name = "Pretraga" }).ClickAsync();
            await page.WaitForSelectorAsync(".container");
            var turnirLocator = page.Locator("ul.turnir-list > li")
            .Locator("app-turnir")
            .Locator("text=Novokreirani turnir");

            var turnirCount = await turnirLocator.CountAsync();
            Assert.IsTrue(turnirCount == 0);

            await page.GetByRole(AriaRole.Link, new() { Name = "Kreiraj turnir" }).ClickAsync();

            await page.GetByLabel("Naziv:").ClickAsync();
            await page.GetByLabel("Naziv:").FillAsync("Novokreirani turnir");
            await page.GetByLabel("Datum održavanja:").FillAsync("2024-09-18");
            await page.GetByLabel("Mesto održavanja:").ClickAsync();
            await page.GetByLabel("Mesto održavanja:").FillAsync("Nis");
            await page.GetByLabel("Broj timova:").ClickAsync();
            await page.GetByLabel("Broj timova:").FillAsync("16");
            await page.GetByLabel("Nagrada pobedniku:").ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Kreiraj Turnir" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Pocetna" }).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Pretraga" }).ClickAsync();
            await page.WaitForSelectorAsync(".container");
            turnirLocator = page.Locator("ul.turnir-list > li")
            .Locator("app-turnir")
            .Locator("text=Novokreirani turnir");

            await page.ScreenshotAsync(new() { Path = "../../../Slike/kreiraniTurnir.png" });
            turnirCount = await turnirLocator.CountAsync();
            Assert.IsTrue(turnirCount == 1);
        }

        [Test]
        public async Task ObrisiTurnir_PrekoPocetne()
        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/");
            await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();

            await page.GetByLabel("Korisničko ime:").ClickAsync();
            await page.GetByLabel("Korisničko ime:").FillAsync("dzontra");
            await page.GetByLabel("Lozinka:").FillAsync("dzontric");
            await page.GetByRole(AriaRole.Button, new() { Name = "Prijavi se" }).ClickAsync();

            await page.GetByRole(AriaRole.Button, new() { Name = "Pretraga" }).ClickAsync();
            await page.WaitForSelectorAsync(".container");

            var turnirLocator = page.Locator("ul.turnir-list > li:has(app-turnir:has-text('Turnir za brisanje'))");
            var turnirCount = await turnirLocator.CountAsync();
            Assert.IsTrue(turnirCount > 0, "Turnir 'Turnir za brisanje' nije pronađen pre brisanja.");

            await turnirLocator.ClickAsync();


            await turnirLocator.GetByRole(AriaRole.Button, new() { Name = "Obrisi turnir" }).ClickAsync();

            await page.GetByRole(AriaRole.Button, new() { Name = "Pretraga" }).ClickAsync();
            await page.WaitForSelectorAsync(".container");

            turnirCount = await page.Locator("ul.turnir-list > li:has(app-turnir:has-text('Turnir za brisanje'))").CountAsync();
            Assert.IsTrue(turnirCount == 0, "Turnir 'Turnir za brisanje' je još uvek prisutan nakon brisanja.");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/deleteTurnir_pocetna.png" });
        }
        [Test]
        public async Task ObrisiTurnir_Preko_MojihTurnira()
        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/");
            await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();

            await page.GetByLabel("Korisničko ime:").ClickAsync();
            await page.GetByLabel("Korisničko ime:").FillAsync("dzontra");
            await page.GetByLabel("Lozinka:").FillAsync("dzontric");
            await page.GetByRole(AriaRole.Button, new() { Name = "Prijavi se" }).ClickAsync();

            await page.GetByRole(AriaRole.Link, new() { Name = "Moji turniri" }).ClickAsync();
            var turnirLocator = page.Locator("div.turnir")
          .Filter(new() { Has = page.Locator("h2", new() { HasText = "Brisanje turnir - moji turniri" }) });

            var turnirCount = await turnirLocator.CountAsync();
            Assert.IsTrue(turnirCount > 0, "Turnir 'Brisanje turnir - moji turniri' nije pronađen pre brisanja.");
            await turnirLocator.GetByRole(AriaRole.Button, new() { Name = "Obrisi turnir" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Pocetna" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Moji turniri" }).ClickAsync();

            turnirLocator = page.GetByText("Brisanje turnir - moji turniri");
            turnirCount = await turnirLocator.CountAsync();
            Assert.IsTrue(turnirCount == 0, "Turnir 'Brisanje turnir - moji turniri' je pronadjen i nakon brisanja.");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/deleteTurnir_mojiturnir.png" });

        }

    }
}
