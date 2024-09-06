namespace TestiranjeProjekat.DTOs
{
    public class TurnirDTO
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string DatumOdrzavanja { get; set; }
        public string MestoOdrzavanja { get; set; }
        public int MaxBrojTimova { get; set; }
        public int TrenutniBrojTimova { get; set; }
        public int Nagrada { get; set; }
        public int? OrganizatorId { get; set; }
    }
}
