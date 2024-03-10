using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Service;
using TestiranjeProjekat.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

//builder.Services.AddScoped<IDogService, DogService>();
//builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IIgracService,IgracService>();
builder.Services.AddScoped<IOrganizatorService,OrganizatorService>();
builder.Services.AddScoped<ITurnirService, TurnirService>();
builder.Services.AddScoped<IPrijavaService, PrijavaService>();
builder.Services.AddControllers();
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
