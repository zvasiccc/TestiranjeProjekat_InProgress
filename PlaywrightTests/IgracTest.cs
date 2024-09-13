using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Playwright;


namespace PlaywrightTests
{
    public class IgracTest
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
        public async Task PrijaviSe_Igrac_Succes()

        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/login");


            await page.FillAsync("#korisnickoIme", "zeljkoLogin");
            await page.FillAsync("#lozinka", "zeljko");


            await page.ClickAsync("#loginButton");


            await page.WaitForURLAsync("http://localhost:4200/");
            var currentUrl = page.Url;
            Assert.AreEqual("http://localhost:4200/", currentUrl);


            await page.ClickAsync("a:has-text('Profil')");


            await page.WaitForSelectorAsync(".profile-container");
            var korisnickoImeNaStranici = await page.TextContentAsync(".profile-container h1");
            Assert.IsTrue(korisnickoImeNaStranici.Contains("Dobrodošli, zeljkoLogin"));


        }

        [Test]
        public async Task IzmenaPodatakaIgrac_ShouldUpdateUserInfoSuccessfully()
        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/login");


            await page.FillAsync("#korisnickoIme", "igracUpdate");
            await page.FillAsync("#lozinka", "igrac");
            await page.ClickAsync("#loginButton");

            await page.GetByRole(AriaRole.Link, new() { Name = "Profil" }).ClickAsync();
            await page.WaitForSelectorAsync(".profile-container");

            await page.ClickAsync("button:has-text('Uredi')");

            await page.FillAsync("#ime", "novoIme");
            await page.FillAsync("#prezime", "novoPrezime");

            await page.ClickAsync("button:has-text('Promeni podatke')");

            await page.ClickAsync("a:has-text('Odjavi se')");
            await page.GotoAsync("http://localhost:4200/login");

            await page.FillAsync("#korisnickoIme", "igracUpdate");
            await page.FillAsync("#lozinka", "igrac");
            await page.ClickAsync("#loginButton");

            await page.GetByRole(AriaRole.Link, new() { Name = "Profil" }).ClickAsync();

            await page.WaitForSelectorAsync(".profile-container");

            var imeNaStranici = await page.InputValueAsync("#ime");
            var prezimeNaStranici = await page.InputValueAsync("#prezime");
            Assert.AreEqual("novoIme", imeNaStranici);
            Assert.AreEqual("novoPrezime", prezimeNaStranici);
        }
        [Test]
        public async Task RegistracijaIgraca_ShouldRegisterAndLoginSuccessfully()
        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/registracija");

            await page.FillAsync("#korisnickoIme", "noviIgrac");
            await page.FillAsync("#lozinka", "novaLozinka");
            await page.FillAsync("#ime", "Novak");
            await page.FillAsync("#prezime", "Novi");
            await page.ClickAsync("#igrac");
            await page.CheckAsync("#vodjaTima");

            await page.ClickAsync("#RegistracijaButton");

            await page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions { Timeout = 60000 });

            await page.GotoAsync("http://localhost:4200/login");
            var currentUrl = page.Url;
            Assert.AreEqual("http://localhost:4200/login", currentUrl);

            await page.FillAsync("#korisnickoIme", "noviIgrac");
            await page.FillAsync("#lozinka", "novaLozinka");

            await page.ClickAsync("#loginButton");

            await page.WaitForURLAsync("http://localhost:4200/");
            currentUrl = page.Url;
            Assert.AreEqual("http://localhost:4200/", currentUrl);
            await page.ClickAsync("a:has-text('Profil')");
            await page.WaitForSelectorAsync(".profile-container");
            var korisnickoImeNaStranici = await page.TextContentAsync(".profile-container h1");
            Assert.IsTrue(korisnickoImeNaStranici.Contains("Dobrodošli, noviIgrac"));

        }

    }
}


