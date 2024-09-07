using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EndToEnd.Tests
{
    public class UserProfileTests
    {
        private IPage _page;
        private IBrowser _browser;

        [SetUp]
        public async Task Setup()
        {
            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
            _page = await _browser.NewPageAsync();
        }

        [Test]
        public async Task TestUserProfilePage()
        {
            await _page.GotoAsync("http://localhost:4200");
            Assert.AreEqual("My App", await _page.TitleAsync());
        }

        [TearDown]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
        }
    }
}
