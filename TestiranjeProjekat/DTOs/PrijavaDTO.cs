namespace TestiranjeProjekat.DTOs
{
    public class PrijavaDTO
    {
        public string NazivTima { get; set; }
        public int PotrebanBrojSlusalica { get; set; }
        public int PotrebanBrojRacunara { get; set; }
        public int PotrebanBrojTastatura { get; set; }
        public int PotrebanBrojMiseva { get; set; }
        public List<int> IgraciId { get; set; }
        public int TurnirId { get; set; }
    }
}
