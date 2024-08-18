using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
// Dodaj konfiguraciju za JSON serializer
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

//builder.Services.AddScoped<IDogService, DogService>();
//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddScoped<IIgracService,IgracService>();
//builder.Services.AddScoped<IOrganizatorService,OrganizatorService>();
//builder.Services.AddScoped<ITurnirService, TurnirService>();
//builder.Services.AddScoped<IPrijavaService, PrijavaService>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.IgnoreNullValues = true; // Ignori�e null vrednosti
        options.JsonSerializerOptions.IgnoreReadOnlyProperties = true; // Ignori�e samo za �itanje svojstva
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
