namespace TestiranjeProjekat.Models
{
    public class Turnir
    {
        public int Id { get; set; }
        public string Naziv {  get; set; }  
        public string DatumOdrzavanja {  get; set; }
        public string MestoOdrzavanja { get; set; }
        public int MaxBrojTimova {  get; set; }
        public int Nagrada {  get; set; }
        public Organizator Organizator { get; set; }
        public List<Prijava>? Prijave { get;set; }

    }
}
