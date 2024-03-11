namespace TestiranjeProjekat.Models
{
    public class Prijava
    {
        public int Id { get; set; }
        public string NazivTima {  get; set; }
        public int PotrebanBrojSlusalica {  get; set; }
        public int PotrebanBrojRacunara { get; set; }
        public int PotrebanBrojTastatura {  get; set; }
        public int PotrebanBrojMiseva { get; set; }
        public List<PrijavaIgracSpoj> Igraci { get; set; }
        public Turnir Turnir { get; set; }
    }
}
