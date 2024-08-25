using TestiranjeProjekat.Models;

namespace Backend.Tests
{
    public class TestDataInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Ensure the database is cleared before seeding
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var igraci = new List<Igrac>
            {
                new Igrac{Id=1,KorisnickoIme="nikola1",Lozinka="111",Ime="Nikola",Prezime="Milosevic",VodjaTima=true},
                new Igrac{Id=2,KorisnickoIme="petar2",Lozinka="222",Ime="Nemanja",Prezime="Petrovic",VodjaTima=false},
                new Igrac{Id=3, KorisnickoIme="marko3", Lozinka="333", Ime="Marko", Prezime="Markovic", VodjaTima=true},
                new Igrac{Id=4, KorisnickoIme="jovan4", Lozinka="444", Ime="Jovan", Prezime="Jovanovic", VodjaTima=false},
                new Igrac{Id=5, KorisnickoIme="stefan5", Lozinka="555", Ime="Stefan", Prezime="Stefanovic", VodjaTima=true},
                new Igrac{Id=6, KorisnickoIme="dusan6", Lozinka="666", Ime="Dusan", Prezime="Dusanovic", VodjaTima=false},
                new Igrac{Id=7, KorisnickoIme="aleksandar7", Lozinka="777", Ime="Aleksandar", Prezime="Aleksic", VodjaTima=true},
                new Igrac{Id=8, KorisnickoIme="ivan8", Lozinka="888", Ime="Ivan", Prezime="Ivanovic", VodjaTima=false},
                new Igrac{Id=9, KorisnickoIme="vladimir9", Lozinka="999", Ime="Vladimir", Prezime="Vladic", VodjaTima=true},
                new Igrac{Id=10, KorisnickoIme="milica10", Lozinka="000", Ime="Milica", Prezime="Miletic", VodjaTima=false},
            };
            var organizatori = new List<Organizator>
            {
                new Organizator { Id = 1, KorisnickoIme = "organizator1", Lozinka = "lozinka1", Ime = "Marko", Prezime = "Markovic" },
                new Organizator { Id = 2, KorisnickoIme = "organizator2", Lozinka = "lozinka2", Ime = "Jovan", Prezime = "Jovanovic" },
                new Organizator { Id = 3, KorisnickoIme = "organizator3", Lozinka = "lozinka3", Ime = "Stefan", Prezime = "Stefanovic" },
                new Organizator { Id = 4, KorisnickoIme = "organizator4", Lozinka = "lozinka4", Ime = "Nemanja", Prezime = "Nemanjic" },
                new Organizator { Id = 5, KorisnickoIme = "organizator5", Lozinka = "lozinka5", Ime = "Milan", Prezime = "Milic" }
            };

            var turniri = new List<Turnir>
            {
                new Turnir { Id = 1, Naziv = "Turnir 1", DatumOdrzavanja = "2024-09-01", MestoOdrzavanja = "Beograd", MaxBrojTimova = 16, TrenutniBrojTimova = 8, Nagrada = 50000, Organizator = organizatori[0] },
                new Turnir { Id = 2, Naziv = "Turnir 2", DatumOdrzavanja = "2024-09-10", MestoOdrzavanja = "Novi Sad", MaxBrojTimova = 8, TrenutniBrojTimova = 4, Nagrada = 30000, Organizator = organizatori[1] },
                new Turnir { Id = 3, Naziv = "Turnir 3", DatumOdrzavanja = "2024-09-15", MestoOdrzavanja = "Nis", MaxBrojTimova = 10, TrenutniBrojTimova = 5, Nagrada = 40000, Organizator = organizatori[2] },
                new Turnir { Id = 4, Naziv = "Turnir 4", DatumOdrzavanja = "2024-09-20", MestoOdrzavanja = "Kragujevac", MaxBrojTimova = 12, TrenutniBrojTimova = 6, Nagrada = 35000, Organizator = organizatori[3] },
                new Turnir { Id = 5, Naziv = "Turnir 5", DatumOdrzavanja = "2024-09-25", MestoOdrzavanja = "Subotica", MaxBrojTimova = 14, TrenutniBrojTimova = 7, Nagrada = 45000, Organizator = organizatori[4] }
            };
            //dodavanje turnira u organizatorovu listu turnira
            foreach (var turnir in turniri)
            {
                var organizator = organizatori.FirstOrDefault(o => o.Id == turnir.Organizator.Id);
                if (organizator != null)
                {
                    if (organizator.Turniri == null)
                    {
                        organizator.Turniri = new List<Turnir>();
                    }
                    organizator.Turniri.Add(turnir);
                }
            }
            var prijave = new List<Prijava>
            {
                new Prijava{Id=1,NazivTima="Tim1",PotrebanBrojSlusalica=2,PotrebanBrojRacunara=2,PotrebanBrojTastatura=2,PotrebanBrojMiseva=1, Igraci = new List<PrijavaIgracSpoj>{ new PrijavaIgracSpoj{ Igrac = igraci[0] },new PrijavaIgracSpoj{Igrac=igraci[1]},new PrijavaIgracSpoj{Igrac=igraci[5]},new PrijavaIgracSpoj{Igrac=igraci[9]} }},
                new Prijava{Id=2,NazivTima="Tim2",PotrebanBrojSlusalica=2,PotrebanBrojRacunara=3,PotrebanBrojTastatura=1,PotrebanBrojMiseva=1, Igraci = new List<PrijavaIgracSpoj>{ new PrijavaIgracSpoj{ Igrac = igraci[1] },new PrijavaIgracSpoj{Igrac=igraci[1]},new PrijavaIgracSpoj{Igrac=igraci[7]},new PrijavaIgracSpoj{Igrac=igraci[8]} }},
            };
            //dodavanje prijave u listu prijava igraca
            foreach (var prijava in prijave)
            {
                foreach (var prijavaIgracSpoj in prijava.Igraci)
                {
                    var igrac = igraci.FirstOrDefault(i => i.Id == prijavaIgracSpoj.Igrac.Id);
                    if (igrac != null)
                    {
                        if (igrac.Prijave == null)
                        {
                            igrac.Prijave = new List<PrijavaIgracSpoj>();
                        }
                        igrac.Prijave.Add(prijavaIgracSpoj);
                    }
                }
            }
            context.Igraci.AddRange(igraci);
            context.Organizatori.AddRange(organizatori);
            context.Turniri.AddRange(turniri);
            context.Prijave.AddRange(prijave);
            context.SaveChanges();
        }
    }
}