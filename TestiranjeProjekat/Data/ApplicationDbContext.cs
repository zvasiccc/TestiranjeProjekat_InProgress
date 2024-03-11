
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using TestiranjeProjekat.Models;

namespace TestiranjeProjekat.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        //public DbSet<User> Users{ get; set; }
        //public DbSet<Dog> Dogs{ get; set; }
        public DbSet<Igrac> Igraci { get; set; }
        public DbSet<Organizator> Organizatori { get; set; }
        public DbSet<Turnir> Turniri { get; set; }
        public DbSet<Prijava> Prijave { get; set; }
        public DbSet<PrijavaIgracSpoj> PrijavaIgracSpoj { get; set; }
    
    }
}
