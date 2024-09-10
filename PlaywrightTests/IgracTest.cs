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
                Headless = false, // Pokreće preglednik sa UI-jem, ako želite bez UI-ja, postavite na `true`
            });
        }

        [TearDown]
        public async Task Teardown()
        {
            await _browser.CloseAsync();
            _playwright.Dispose();
        }


        [Test]
        public async Task PrijaviSe_ShouldLoginSuccessfully_WithCorrectCredentials()

        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/login");

            // Unos korisničkog imena i lozinke
            await page.FillAsync("#korisnickoIme", "zeljko123");
            await page.FillAsync("#lozinka", "zeljko"); // Zameni sa stvarnim ID-om

            // Klik na dugme za prijavu
            await page.ClickAsync("#loginButton");


            await page.WaitForURLAsync("http://localhost:4200/");
            var currentUrl = page.Url;
            Assert.AreEqual("http://localhost:4200/", currentUrl);

            // Klik na link za navigaciju na profil
            await page.ClickAsync("a:has-text('Profil')");

            // Provera da li se prikazuje profil
            await page.WaitForSelectorAsync(".profile-container"); // Očekuje se da profil bude prikazan
            var korisnickoImeNaStranici = await page.TextContentAsync(".profile-container h1");
            Assert.IsTrue(korisnickoImeNaStranici.Contains("Dobrodošli, zeljko123")); // Proveri da li je korisničko ime ispravno

            // Ako je potrebno, dodaj dodatne provere za profil
        }

        [Test]
        public async Task IzmenaPodataka_ShouldUpdateUserInfoSuccessfully()
        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/login");

            // Unos korisničkog imena i lozinke
            await page.FillAsync("#korisnickoIme", "proba123"); // Zameni sa stvarnim ID-om
            await page.FillAsync("#lozinka", "proba"); // Zameni sa stvarnim ID-om
            await page.ClickAsync("#loginButton"); // Zameni sa stvarnim selektorom za dugme prijave


            await page.WaitForURLAsync("http://localhost:4200/");
            var currentUrl = page.Url;
            Assert.AreEqual("http://localhost:4200/", currentUrl);

            // Klik na link za navigaciju na profil
            await page.ClickAsync("a:has-text('Profil')");

            // Provera da li se prikazuje profil
            await page.WaitForSelectorAsync(".profile-container");

            // Klik na dugme za omogućavanje izmene
            await page.ClickAsync("button:has-text('Uredi')");

            // Izmena imena i prezimena
            await page.FillAsync("#ime", "novoIme");
            await page.FillAsync("#prezime", "novoPrezime");

            // Klik na dugme za potvrdu izmene
            await page.ClickAsync("button:has-text('Promeni podatke')");

            // Izloguj se korisnik
            await page.ClickAsync("a:has-text('Odjavi se')");
            await page.GotoAsync("http://localhost:4200/login");


            // Ponovo se prijavi sa istim akreditivima
            await page.FillAsync("#korisnickoIme", "proba123"); // Zameni sa stvarnim ID-om
            await page.FillAsync("#lozinka", "proba");
            await page.ClickAsync("#loginButton"); // Zameni sa stvarnim selektorom za dugme prijave

            // Provera da li je korisnik uspešno preusmeren na početnu stranicu
            await page.WaitForURLAsync("http://localhost:4200/");
            currentUrl = page.Url;
            Assert.AreEqual("http://localhost:4200/", currentUrl);

            // Klik na link za navigaciju na profil
            await page.ClickAsync("a[routerLink='profil']"); // Klik na link za profil

            // Provera da li se prikazuje profil
            await page.WaitForSelectorAsync(".profile-container"); // Očekuje se da profil bude prikazan

            // Proveri da li su podaci ažurirani
            var korisnickoImeNaStranici = await page.InputValueAsync("#korisnickoIme");
            var imeNaStranici = await page.InputValueAsync("#ime");
            var prezimeNaStranici = await page.InputValueAsync("#prezime");

            Assert.AreEqual("novoKorisnickoIme", korisnickoImeNaStranici); // Proveri novo korisničko ime
            Assert.AreEqual("novoIme", imeNaStranici); // Proveri novo ime
            Assert.AreEqual("novoPrezime", prezimeNaStranici); // Proveri novo prezime
        }
        [Test]
        public async Task RegistracijaIgraca_ShouldRegisterAndLoginSuccessfully()
        {
            var page = await _browser.NewPageAsync();
            await page.GotoAsync("http://localhost:4200/registracija");

            // Izaberi opciju "Igrač" u formi za registraciju

            // Unesi podatke za registraciju igrača
            await page.FillAsync("#korisnickoIme", "noviIgrac");
            await page.FillAsync("#lozinka", "novaLozinka");
            await page.FillAsync("#ime", "Novak");
            await page.FillAsync("#prezime", "Novi");
            await page.ClickAsync("#igrac");

            // Ako je potrebno, označi checkbox "Vodja tima"
            await page.CheckAsync("#vodjaTima");

            // Klik na dugme za registraciju igrača
            await page.ClickAsync("#RegistracijaButton");

            // Sačekaj da se registracija završi i proveri da li je korisnik preusmeren
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle, new PageWaitForLoadStateOptions { Timeout = 60000 });


            await page.GotoAsync("http://localhost:4200/login");
            var currentUrl = page.Url;
            Assert.AreEqual("http://localhost:4200/login", currentUrl);


            // Unesi podatke za prijavu
            await page.FillAsync("#korisnickoIme", "noviIgrac");
            await page.FillAsync("#lozinka", "novaLozinka");

            // Klik na dugme za prijavu
            await page.ClickAsync("#loginButton");

            // Sačekaj da se prijava završi i proveri da li je korisnik preusmeren na početnu stranicu
            await page.WaitForURLAsync("http://localhost:4200/");
            currentUrl = page.Url;
            Assert.AreEqual("http://localhost:4200/", currentUrl);
            await page.ClickAsync("a:has-text('Profil')");

            // Provera da li se prikazuje profil
            await page.WaitForSelectorAsync(".profile-container"); // Očekuje se da profil bude prikazan
            var korisnickoImeNaStranici = await page.TextContentAsync(".profile-container h1");
            Assert.IsTrue(korisnickoImeNaStranici.Contains("Dobrodošli, noviIgrac")); // Proveri da li je korisničko ime ispravno


        }

    }
}


