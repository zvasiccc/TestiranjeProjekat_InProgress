using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Playwright;


namespace PlaywrightTests
{
    public class PrijavaTest
    {
        private IPlaywright _playwright;
        private IBrowser _browser;

        [SetUp]
        public async Task Setup()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
            });
        }

        [TearDown]
        public async Task Teardown()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }
        [Test]
        public async Task CreateRegistration()
        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/login");

            await page.GetByLabel("Korisničko ime:").ClickAsync();
            await page.GetByLabel("Korisničko ime:").FillAsync("proba1");
            await page.GetByLabel("Lozinka:").ClickAsync();
            await page.GetByLabel("Lozinka:").FillAsync("proba");
            await page.GetByRole(AriaRole.Button, new() { Name = "Prijavi se" }).ClickAsync();

            await page.GetByRole(AriaRole.Button, new() { Name = "Pretraga" }).ClickAsync();
            await page.Locator("div.turnir")
             .Filter(new() { Has = page.Locator("h2", new() { HasText = "Turnir za kreiranje prijave" }) })
            .GetByRole(AriaRole.Button, new() { Name = "Prijavi se" })
            .ClickAsync();


            await page.Locator("li").Filter(new() { HasText = "Korisnicko ime: milos:" }).GetByRole(AriaRole.Button).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Preference" }).ClickAsync();


            await page.GetByLabel("Potreban broj tastatura:").ClickAsync();
            await page.GetByLabel("Potreban broj tastatura:").FillAsync("3");
            await page.GetByLabel("Potreban broj miševa:").ClickAsync();
            await page.GetByLabel("Potreban broj miševa:").FillAsync("1");
            await page.GetByLabel("Potreban broj računara:").FillAsync("2");
            await page.GetByLabel("Potreban broj tastatura:").FillAsync("1");

            await page.GetByRole(AriaRole.Button, new() { Name = "Potvrdi" }).ClickAsync();

            await page.GetByLabel("naziv tima").ClickAsync();
            await page.GetByLabel("naziv tima").FillAsync("Nas tim 123");

            await page.GetByRole(AriaRole.Button, new() { Name = "Posalji prijavu" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Profil" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Pocetna" }).ClickAsync();

            await page.GetByRole(AriaRole.Button, new() { Name = "Pretraga" }).ClickAsync();
            var izbrisiPrijavuButtonCount = await page.Locator("div.turnir")
        .Filter(new() { Has = page.Locator("h2", new() { HasText = "Turnir za kreiranje prijave" }) })
        .GetByRole(AriaRole.Button, new() { Name = "Izbrisi prijavu" })
        .CountAsync();

            Assert.IsTrue(izbrisiPrijavuButtonCount > 0, "Dugme 'Izbrisi prijavu' nije prikazano nakon prijave.");

        }
        [Test]
        public async Task BrisanjePrijave()
        {
            var page = await _browser.NewPageAsync();

            await page.GotoAsync("http://localhost:4200/login");
            await page.GetByLabel("Korisničko ime:").ClickAsync();
            await page.GetByLabel("Korisničko ime:").FillAsync("igrac1");
            await page.GetByLabel("Lozinka:").ClickAsync();
            await page.GetByLabel("Lozinka:").FillAsync("igrac");
            await page.GetByRole(AriaRole.Button, new() { Name = "Prijavi se" }).ClickAsync();

            await page.GetByRole(AriaRole.Button, new() { Name = "Pretraga" }).ClickAsync();
            var izbrisiPrijavuButtonCount0 = await page.Locator("div.turnir")
            .Filter(new() { Has = page.Locator("h2", new() { HasText = "Turnir u Berilovac" }) })
            .GetByRole(AriaRole.Button, new() { Name = "Izbrisi prijavu" })
            .CountAsync();

            Assert.IsTrue(izbrisiPrijavuButtonCount0 == 0);
            await page.Locator("div.turnir")
             .Filter(new() { Has = page.Locator("h2", new() { HasText = "Turnir u Berilovac" }) })
            .GetByRole(AriaRole.Button, new() { Name = "Prijavi se" })
            .ClickAsync();


            await page.Locator("li").Filter(new() { HasText = "Korisnicko ime: igrac2Ime:" }).GetByRole(AriaRole.Button).ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Preference" }).ClickAsync();

            await page.GetByLabel("Potreban broj miševa:").ClickAsync();
            await page.GetByLabel("Potreban broj miševa:").FillAsync("1");
            await page.GetByRole(AriaRole.Button, new() { Name = "Potvrdi" }).ClickAsync();

            await page.GetByLabel("naziv tima").ClickAsync();
            await page.GetByLabel("naziv tima").FillAsync("Ovu prijavu cu da obrisem");

            await page.GetByRole(AriaRole.Button, new() { Name = "Posalji prijavu" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Profil" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Pocetna" }).ClickAsync();
            page.Dialog += async (sender, dialog) =>
            {
                Console.WriteLine($"Dialog message: {dialog.Message}");
                await dialog.DismissAsync();
            };
            var izbrisiPrijavuButtonCount1 = await page.Locator("div.turnir")
            .Filter(new() { Has = page.Locator("h2", new() { HasText = "Turnir u Berilovac" }) })
            .GetByRole(AriaRole.Button, new() { Name = "Izbrisi prijavu" })
            .CountAsync();
            Assert.IsTrue(izbrisiPrijavuButtonCount1 == 1);

            await page.GetByRole(AriaRole.Button, new() { Name = "Izbrisi prijavu" }).ClickAsync();

            await page.GetByRole(AriaRole.Link, new() { Name = "Profil" }).ClickAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Pocetna" }).ClickAsync();
            izbrisiPrijavuButtonCount0 = await page.Locator("div.turnir")
           .Filter(new() { Has = page.Locator("h2", new() { HasText = "Turnir u Berilovac" }) })
           .GetByRole(AriaRole.Button, new() { Name = "Izbrisi prijavu" })
           .CountAsync();
            Assert.IsTrue(izbrisiPrijavuButtonCount0 == 0);
            await page.ScreenshotAsync(new() { Path = "../../../Slike/BrisanjePrijave.png" });

        }
        [Test]
        public async Task IgracProverava_PrijavljeneTimove_Na_Turniru_()
        {
            var page = await _browser.NewPageAsync();

            await page.GotoAsync("http://localhost:4200/login");


            await page.GetByLabel("Korisničko ime:").ClickAsync();
            await page.GetByLabel("Korisničko ime:").FillAsync("proba1");
            await page.GetByLabel("Korisničko ime:").PressAsync("Tab");
            await page.GetByLabel("Lozinka:").FillAsync("proba");
            await page.GetByLabel("Lozinka:").PressAsync("Enter");


            await page.GetByRole(AriaRole.Button, new() { Name = "Pretraga" }).ClickAsync();
            await page.Locator("div.turnir")
           .Filter(new() { Has = page.Locator("h2", new() { HasText = "Turnir u Rzani" }) })
           .GetByRole(AriaRole.Button, new() { Name = "Prijavljeni timovi" }).ClickAsync();


            var prijavaList = await page.QuerySelectorAllAsync("ul > li");
            Assert.IsTrue(prijavaList.Count > 0, "Nema prikazanih prijava.");
            await page.ScreenshotAsync(new() { Path = "../../../Slike/Igrac_TimoviNaTurniru.png" });

        }
        [Test]
        public async Task IgracProverava_Saigrace_Na_Turniru_()
        {
            var page = await _browser.NewPageAsync();

            await page.GotoAsync("http://localhost:4200/login");


            await page.GetByLabel("Korisničko ime:").ClickAsync();
            await page.GetByLabel("Korisničko ime:").FillAsync("proba1");
            await page.GetByLabel("Korisničko ime:").PressAsync("Tab");
            await page.GetByLabel("Lozinka:").FillAsync("proba");
            await page.GetByLabel("Lozinka:").PressAsync("Enter");


            await page.GetByRole(AriaRole.Button, new() { Name = "Pretraga" }).ClickAsync();
            await page.Locator("div.turnir")
           .Filter(new() { Has = page.Locator("h2", new() { HasText = "Turnir u Rzani" }) })
          .GetByRole(AriaRole.Button, new() { Name = "Vidi saigrace" }).ClickAsync();
            var igraciList = await page.QuerySelectorAllAsync("ul.my-list > li");

            Assert.IsTrue(igraciList.Count > 0, "Nema prikazanih saigrača.");
            foreach (var igrac in igraciList)
            {
                var igracElement = await igrac.QuerySelectorAsync("app-igrac");
                var korisnickoIme = await igracElement.QuerySelectorAsync("p:nth-of-type(1)");
                var ime = await igracElement.QuerySelectorAsync("p:nth-of-type(3)");
                var prezime = await igracElement.QuerySelectorAsync("p:nth-of-type(4)");

                var korisnickoImeText = await korisnickoIme.InnerTextAsync();
                var imeText = await ime.InnerTextAsync();
                var prezimeText = await prezime.InnerTextAsync();

                Assert.IsFalse(string.IsNullOrWhiteSpace(korisnickoImeText), "Korisnicko ime saigrača je prazno.");
                Assert.IsFalse(string.IsNullOrWhiteSpace(imeText), "Ime saigrača je prazno.");
                Assert.IsFalse(string.IsNullOrWhiteSpace(prezimeText), "Prezime saigrača je prazno.");
            }
            await page.ScreenshotAsync(new() { Path = "../../../Slike/saigraciNaTurniru.png" });


        }



    }
}