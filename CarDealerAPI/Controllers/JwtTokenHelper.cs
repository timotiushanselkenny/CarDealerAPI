using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CarDealerAPI.Controllers
{
    public static class JwtTokenHelper
    {
        public static string GenerateJwtToken(string username, int DealerID, string secretKey)
        {
            var claims = new[]
{
                new Claim(ClaimTypes.Name, username),
                new Claim("DealerID", DealerID.ToString()),
                new Claim(ClaimTypes.Role, "Dealer")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "CarDealer",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
        public static string ReturnDealerID(string authHeader)
        {
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return "Authorization header is missing or invalid.";
            }

            // Extract the token
            var token = authHeader.Substring("Bearer ".Length);

            try
            {
                // Initialize the JWT handler
                var tokenHandler = new JwtSecurityTokenHandler();

                if (tokenHandler.CanReadToken(token))
                {
                    // Parse the token
                    var jwtToken = tokenHandler.ReadJwtToken(token);

                    // Extract claims
                    var claims = jwtToken.Claims.Select(c => new { c.Type, c.Value });

                    return claims.Where(x => x.Type == "DealerID").First().Value;
                }
                return "Authorization header is missing or invalid.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
