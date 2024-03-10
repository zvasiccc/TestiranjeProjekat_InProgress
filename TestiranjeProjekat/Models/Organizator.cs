namespace TestiranjeProjekat.Models
{
    public class Organizator
    {
        public int Id { get; set; }
        public string KorisnickoIme{get;set;}
        public string Lozinka { get;set; }
        public string Ime {  get;set; }
        public string Prezime {  get;set; }
        public List<Turnir> Turniri { get; set; } = new List<Turnir>();

    }
}
