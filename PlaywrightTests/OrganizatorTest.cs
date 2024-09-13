using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Playwright;


namespace PlaywrightTests
{
    public class OrganizatorTest
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
        public async Task PrijaviSe_Organizator_Success()

        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/login");

            await page.FillAsync("#korisnickoIme", "organizatorLogin");
            await page.FillAsync("#lozinka", "organizator");

            await page.ClickAsync("#loginButton");

            await page.ClickAsync("a:has-text('Profil')");

            await page.WaitForSelectorAsync(".profile-container");
            var korisnickoImeNaStranici = await page.TextContentAsync(".profile-container h1");
            Assert.IsTrue(korisnickoImeNaStranici.Contains("Dobrodošli, organizatorLogin"));
            await page.ScreenshotAsync(new() { Path = "../../../Slike/prijavljeniOrganizator.png" });
        }

        [Test]
        public async Task IzmenaPodatakaOrganizator_ShouldUpdateUserInfoSuccessfully()
        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/login");


            await page.FillAsync("#korisnickoIme", "organizatorUpdate");
            await page.FillAsync("#lozinka", "organizator");
            await page.ClickAsync("#loginButton");

            await page.GetByRole(AriaRole.Link, new() { Name = "Profil" }).ClickAsync();
            await page.WaitForSelectorAsync(".profile-container");

            await page.ClickAsync("button:has-text('Uredi')");

            await page.FillAsync("#ime", "novoImeOrganizator");
            await page.FillAsync("#prezime", "novoPrezimeOrganizator");

            await page.ClickAsync("button:has-text('Promeni podatke')");

            await page.ClickAsync("a:has-text('Odjavi se')");
            await page.GotoAsync("http://localhost:4200/login");

            await page.FillAsync("#korisnickoIme", "organizatorUpdate");
            await page.FillAsync("#lozinka", "organizator");
            await page.ClickAsync("#loginButton");

            await page.GetByRole(AriaRole.Link, new() { Name = "Profil" }).ClickAsync();

            await page.WaitForSelectorAsync(".profile-container");

            var imeNaStranici = await page.InputValueAsync("#ime");
            var prezimeNaStranici = await page.InputValueAsync("#prezime");
            Assert.AreEqual("novoImeOrganizator", imeNaStranici);
            Assert.AreEqual("novoPrezimeOrganizator", prezimeNaStranici);
            await page.ScreenshotAsync(new() { Path = "../../../Slike/azuriraniPodaciOrganizator.png" });
        }
        [Test]
        public async Task RegistracijaOrganizatora_ShouldRegisterAndLoginSuccessfully()
        {
            var page = await _browser.NewPageAsync();

            await page.GotoAsync("http://localhost:4200/");

            await page.GetByRole(AriaRole.Link, new() { Name = "Registracija" }).ClickAsync();

            await page.Locator("#organizator").CheckAsync();

            await page.GetByLabel("Korisničko ime:").ClickAsync();
            await page.GetByLabel("Korisničko ime:").FillAsync("noviOrganizator");
            await page.GetByLabel("Korisničko ime:").PressAsync("Tab");

            await page.GetByLabel("Lozinka:").FillAsync("organizator");
            await page.GetByLabel("Lozinka:").PressAsync("Tab");

            await page.GetByLabel("Ime:", new() { Exact = true }).FillAsync("Milos");
            await page.GetByLabel("Ime:", new() { Exact = true }).PressAsync("Tab");

            await page.GetByLabel("Prezime:").FillAsync("Jovanovic");

            await page.GetByRole(AriaRole.Button, new() { Name = "Registruj se" }).ClickAsync();

            await page.GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();

            await page.GetByLabel("Korisničko ime:").ClickAsync();
            await page.GetByLabel("Korisničko ime:").FillAsync("noviOrganizator");
            await page.GetByLabel("Lozinka:").ClickAsync();
            await page.GetByLabel("Lozinka:").FillAsync("organizator");

            await page.GetByRole(AriaRole.Button, new() { Name = "Prijavi se" }).ClickAsync();

            await page.GetByRole(AriaRole.Link, new() { Name = "Profil" }).ClickAsync();
            await page.WaitForSelectorAsync(".profile-container");

            var korisnickoImeNaStranici = await page.TextContentAsync(".profile-container h1");
            Assert.IsTrue(korisnickoImeNaStranici.Contains("Dobrodošli, noviOrganizator"));
            await page.ScreenshotAsync(new() { Path = "../../../Slike/NovoRegistrovaniOrganizator.png" });

        }


    }
}


