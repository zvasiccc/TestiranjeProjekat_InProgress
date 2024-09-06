using System.Text.Json.Serialization;
using TestiranjeProjekat.Models;

namespace TestiranjeProjekat.DTOs
{
    public class IgracDTO
    {
        //[JsonIgnore]
        public int Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public bool VodjaTima { get; set; }
 
    }
}
