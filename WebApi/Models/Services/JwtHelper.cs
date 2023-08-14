using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Models.Services
{
    public static class JwtHelper
    {
        public static string GenerateToken(UserModel model, IConfiguration configuration)
        {
            var issuer = configuration["Jwt:Issuer"];
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim("Id", model.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Name, model.UserName),
                        new Claim(JwtRegisteredClaimNames.Email, model.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    }),
                Expires = DateTime.Now.AddMinutes(20),
                Issuer = issuer,
                Audience = issuer,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
    }
}
