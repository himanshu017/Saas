using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdminPanel.API.AuthTokenService
{
    public class TokenAuth : ITokenAuth
    {
        private readonly string key;
        public TokenAuth(string key)
        {
            this.key = key;
        }
        public async Task<LoginResponse> GenerateToken(LoginResponse response)
        {
            // 1. Create Security Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // 2. Create Private Key to Encrypted
            var tokenKey = Encoding.ASCII.GetBytes(key);

            //3. Create JETdescriptor
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, response.FullName),
                        new Claim("UserId", response.UserId.ToString()),
                        new Claim("CompanyId", response.Role == UserTypes.SuperAdmin.ToString() ? "0" : response.CompanyId.ToString()),
                        new Claim(ClaimTypes.Role, response.Role),
                        new Claim(ClaimTypes.Email, response.Email),
                        new Claim("CompanyName", response.CompanyName ?? ""),
                        new Claim("DomainName",response.DomainName??"")
                    }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            //4. Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            response.Token = tokenHandler.WriteToken(token);

            // 5. Return Token from method
            return response;
        }
    }
}
