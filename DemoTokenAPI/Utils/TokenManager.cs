using DemoTokenAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoTokenAPI.Utils
{
    public class TokenManager
    {
        private readonly string _secretKey;

        public TokenManager(IConfiguration config)
        {
            _secretKey = config.GetSection("TokenInfo").GetSection("secret").Value;
        }

        public string GenerateToken(User user)
        {
            //Créer la verify signature
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            //Création du PayLoad (info contenue dans le token)
            Claim[] MyClaims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "admin" : "user"),
                new Claim("UserId", user.Id.ToString())
            };

            //Configurer le token
            JwtSecurityToken jwt = new JwtSecurityToken(
                claims : MyClaims,
                signingCredentials : credentials,
                expires : DateTime.Now.AddDays(1),
                issuer : "https://monapi.com", //nom de domaine émetteur du token
                audience :"https://monsite.com" //nom de domaine consomme le token 
                );

            //Génération du string
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(jwt);
        }
    }
}
