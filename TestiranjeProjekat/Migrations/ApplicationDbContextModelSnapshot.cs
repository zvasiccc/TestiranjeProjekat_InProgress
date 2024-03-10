﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TestiranjeProjekat.Data;

#nullable disable

namespace TestiranjeProjekat.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TestiranjeProjekat.Models.Igrac", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Ime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("KorisnickoIme")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Lozinka")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Prezime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("VodjaTima")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Igraci");
                });

            modelBuilder.Entity("TestiranjeProjekat.Models.Organizator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Ime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("KorisnickoIme")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Lozinka")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Prezime")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Organizatori");
                });

            modelBuilder.Entity("TestiranjeProjekat.Models.Prijava", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("NazivTima")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("potrebanBrojMiseva")
                        .HasColumnType("integer");

                    b.Property<int>("potrebanBrojRacunara")
                        .HasColumnType("integer");

                    b.Property<int>("potrebanBrojSlusalica")
                        .HasColumnType("integer");

                    b.Property<int>("potrebanBrojTastatura")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Prijave");
                });

            modelBuilder.Entity("TestiranjeProjekat.Models.Turnir", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DatumOdrzavanja")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MaxBrojTimova")
                        .HasColumnType("integer");

                    b.Property<string>("MestoOdrzavanja")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Nagrada")
                        .HasColumnType("integer");

                    b.Property<string>("Naziv")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("OrganizatorId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrganizatorId");

                    b.ToTable("Turniri");
                });

            modelBuilder.Entity("TestiranjeProjekat.Models.Turnir", b =>
                {
                    b.HasOne("TestiranjeProjekat.Models.Organizator", "Organizator")
                        .WithMany("Turniri")
                        .HasForeignKey("OrganizatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organizator");
                });

            modelBuilder.Entity("TestiranjeProjekat.Models.Organizator", b =>
                {
                    b.Navigation("Turniri");
                });
#pragma warning restore 612, 618
        }
    }
}
