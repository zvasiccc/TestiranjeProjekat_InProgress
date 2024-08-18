using Microsoft.EntityFrameworkCore;
using TestiranjeProjekat.Models;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Organizator> Organizatori { get; set; }
    public DbSet<Igrac> Igraci { get; set; }
    public DbSet<Turnir> Turniri { get; set; }
    public DbSet<Prijava> Prijave { get; set; }
    public DbSet<PrijavaIgracSpoj> PrijavaIgracSpoj { get; set; }
}