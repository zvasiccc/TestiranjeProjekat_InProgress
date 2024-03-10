using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using TestiranjeProjekat.Data;
using TestiranjeProjekat.Models;

namespace TestiranjeProjekat.Service
{
    public interface IIgracService
    {
        public Task<List<Igrac>> IgraciSaSlicnimKorisnickimImenom(string korisnickoIme);
    }
    public class IgracService:IIgracService
    {
        private readonly ApplicationDbContext _context;
        public IgracService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Igrac>> IgraciSaSlicnimKorisnickimImenom(string korisnickoIme)
        {
            try
            {
  
                var igraci = await _context.Igraci
                                            .Where(i => i.KorisnickoIme.Contains(korisnickoIme))
                                            .ToListAsync();

                if (igraci.Any())
                {
        
                    return igraci;
                }
                else
                {
                  
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        
    }
    }
}
