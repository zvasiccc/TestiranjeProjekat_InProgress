using TestiranjeProjekat.Models;

namespace TestiranjeProjekat.DTOs
{
    public class PrijavaDTO2
    {
        public int Id { get; set; }
        public string NazivTima { get; set; }
        public int PotrebanBrojSlusalica { get; set; }
        public int PotrebanBrojRacunara { get; set; }
        public int PotrebanBrojTastatura { get; set; }
        public int PotrebanBrojMiseva { get; set; }
        public List<IgracDTO> Igraci { get; set; }
        public TurnirDTO Turnir { get; set; }
    }
}
