using Microsoft.AspNetCore.Mvc;
using TestiranjeProjekat.Service;

namespace TestiranjeProjekat.Controllers
{
    [ApiController]
    //Route("[controller]")]
    [Route("[controller]")]
    public class IgracController:ControllerBase
    {
        private readonly IgracService _igracService;
        public IgracController(IgracService igracService)
        {
            _igracService = igracService;
        }
        [HttpGet("korisnickoIme/{korisnickoIme}")]
        public IActionResult igraciSaSlicnimKorisnickimImenom(string korisnickoIme)
        {
            var rezultat = _igracService.igraciSaSlicnimKorisnickimImenom(korisnickoIme);
            return Ok(rezultat);
        }
    }
}
