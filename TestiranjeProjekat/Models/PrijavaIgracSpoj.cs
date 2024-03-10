using System.ComponentModel.DataAnnotations;

namespace TestiranjeProjekat.Models
{
    public class PrijavaIgracSpoj
    {
        public int IgracId { get; set; }
        public int PrijavaId { get; set; }
        public Igrac Igrac { get; set; }
        public Prijava Prijava { get; set; }
        [Key]
        public int Id { get; set; }
    }
}
