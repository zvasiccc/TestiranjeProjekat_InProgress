namespace TestiranjeProjekat.Models
{
    public class Turnir
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string DatumOdrzavanja { get; set; }
        public string MestoOdrzavanja { get; set; }
        public int MaxBrojTimova { get; set; }
        public int TrenutniBrojTimova { get; set; }
        public int Nagrada { get; set; }
        public Organizator? Organizator { get; set; }
        public List<Prijava>? Prijave { get; set; }

        public override bool Equals(object? obj)
        {
            Turnir? t2 = obj as Turnir;
            if (t2 == null)
            {
                return false;
            }

            if (Id == t2.Id)
            {
                return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return 0;
        }
    }
}
