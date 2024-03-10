namespace TestiranjeProjekat.Models
{
    public class Igrac
    {
        public int Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get;set; }
        public string Prezime {  get; set; }
        public bool VodjaTima {  get; set; }
        public List<PrijavaIgracSpoj> Prijave{ get; set; }
    }
}
