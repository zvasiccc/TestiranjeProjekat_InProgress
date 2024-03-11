//using Microsoft.EntityFrameworkCore;
//using System.ComponentModel;
//using TestiranjeProjekat.Data;
//using TestiranjeProjekat.Models;

//namespace TestiranjeProjekat.Service
//{
//    public interface IIgracService
//    {
//        public Task<List<Igrac>> IgraciSaSlicnimKorisnickimImenom(string korisnickoIme);
//        public Task<Igrac> DohvatiIgraca(string korisnickoIme);
//        public Task RegistrujIgraca(Igrac igrac);
//        public void IzmeniPodatkeOIgracu(int igracId, Igrac noviIgrac);
//        public Task<List<Igrac>> vratiSaigrace(int turnirId, int igracId);
//    }
//    public class IgracService : IIgracService
//    {
//        private readonly ApplicationDbContext _context;
//        public IgracService(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public Task<Igrac> DohvatiIgraca(string korisnickoIme)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<List<Igrac>> IgraciSaSlicnimKorisnickimImenom(string korisnickoIme)
//        {
//            try
//            {
  
//                var igraci = await _context.Igraci
//                                            .Where(i => i.KorisnickoIme.Contains(korisnickoIme))
//                                            .ToListAsync();

//                if (igraci.Any())
//                {
        
//                    return igraci;
//                }
//                else
//                {
                  
//                    return null;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex);
//                return null;
//            }
        
//    }

//        public void IzmeniPodatkeOIgracu(int igracId, Igrac noviIgrac)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task RegistrujIgraca(Igrac igrac)
//        {
//            _context.Igraci.Add(igrac);
//            await _context.SaveChangesAsync();
//           // throw new NotImplementedException();
//        }

//        public Task<List<Igrac>> vratiSaigrace(int turnirId, int igracId)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
