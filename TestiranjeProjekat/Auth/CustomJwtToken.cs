// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.IdentityModel.Tokens;

// namespace TestiranjeProjekat.Auth
// {


//     public class CustomJwtToken
//     {
//         public string Username { get; }
//         public string Sub { get; }
//         public string Role { get; }

//         public CustomJwtToken(string username, string sub, string role)
//         {
//             Username = username;
//             Sub = sub;
//             Role = role;
//         }

//         public string GenerateToken()
//         {
//             var claims = new[]
//             {
//             new Claim("username", Username),
//             new Claim("sub", Sub),
//             new Claim("role", Role)
//         };

//             var token = new JwtSecurityToken(
//                 claims: claims,
//                 signingCredentials: new SigningCredentials(
//                     new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
//                     SecurityAlgorithms.HmacSha256)
//             );

//             return new JwtSecurityTokenHandler().WriteToken(token);
//         }
//     }
// }