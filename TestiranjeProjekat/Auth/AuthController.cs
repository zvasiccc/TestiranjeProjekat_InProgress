using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenObject
{
    public string accessToken { get; set; }
    public User korisnik { get; set; }
}
public class User
{
    public int id { get; set; }
    public string korisnickoIme { get; set; }
    public string ime { get; set; }
    public string prezime { get; set; }
    public string role { get; set; }
    public bool vodjaTima { get; set; }

}

public class UserLoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}

[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;


    public AuthController(IConfiguration configuration, AppDbContext appContext)
    {
        _configuration = configuration;
        _context = appContext;
    }



    [HttpPost("login")]
    public async Task<TokenObject> Login([FromBody] UserLoginModel user)
    {


        var player = await _context.Igraci.FirstOrDefaultAsync(p => p.KorisnickoIme == user.Username && p.Lozinka == user.Password);
        var organizator = player == null
            ? await _context.Organizatori.FirstOrDefaultAsync(o => o.KorisnickoIme == user.Username && o.Lozinka == user.Password)
            : null;
        if (player == null && organizator == null)
        {
            throw new Exception();
        }
        if (player != null)
        {
            var token = GenerateJwtToken(user.Username, player.Id.ToString(), "igrac");

            TokenObject o = new TokenObject();
            o.accessToken = token;
            User u = new User();
            u.id = player.Id;
            u.korisnickoIme = player.KorisnickoIme;
            u.ime = player.Ime;
            u.prezime = player.Prezime;
            u.vodjaTima = player.VodjaTima;
            u.role = "igrac";
            o.korisnik = u;
            return o;
        }
        else if (organizator != null)
        {
            var token = GenerateJwtToken(user.Username, organizator.Id.ToString(), "organizator");
            TokenObject o = new TokenObject();
            o.accessToken = token;
            User u = new User();
            u.id = organizator.Id;
            u.korisnickoIme = organizator.KorisnickoIme;
            u.ime = organizator.Ime;
            u.prezime = organizator.Prezime;
            u.role = "organizator";
            o.korisnik = u;
            return o;
        }

        return new TokenObject();
    }

    private string GenerateJwtToken(string username, string sub, string role)
    {
        try
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, sub),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("role", role),
            new Claim("username", username)
        };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            Console.WriteLine("Token generated successfully.");

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating token: {ex.Message}");
            throw;
        }
    }

}
