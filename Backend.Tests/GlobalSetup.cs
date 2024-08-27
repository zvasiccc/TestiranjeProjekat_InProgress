using System.Net.NetworkInformation;
using Backend.Tests;
using Microsoft.EntityFrameworkCore;

[SetUpFixture]
public class GlobalSetup
{
    public static AppDbContext AppContext;
    private static DbContextOptions<AppDbContext> dbContextOptions;
    [OneTimeSetUp]
    public void GlobalSetupMethod()
    {
        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
         .UseInMemoryDatabase("TestDatabase")
         .Options;

        AppContext = new AppDbContext(dbContextOptions);
        TestDataInitializer.Initialize(AppContext);
    }
    [OneTimeTearDown]
    public void TearDown()
    {

        AppContext.Dispose();

    }

}