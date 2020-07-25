
using Azure_Todo_Functions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Legacy_Todo.Services
{
    public class AuthData
    {
        public string Token { get; set; }
        public long TokenExpirationTime { get; set; }
        public string Id { get; set; }
    }

    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string actualPassword, string hashedPassword);
        AuthData GetAuthData(string id);
        bool ValidateToken(HttpRequest req);
    }

    
    public class AuthService : IAuthService
    {
        string jwtSecret;
        int jwtLifespan;
        public AuthService(string jwtSecret, int jwtLifespan)
        {
            this.jwtSecret = jwtSecret;
            this.jwtLifespan = jwtLifespan;
        }

        public bool ValidateToken(HttpRequest req)
        {
            // Check if we have a header.
            if (!req.Headers.ContainsKey("Authorization"))
            {
                return false;
            }

            string authorizationHeader = req.Headers["Authorization"];

            // Check if the value is empty.
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return false;
            }

            if (authorizationHeader.StartsWith("Bearer") && authorizationHeader.Length > 10)
            {
                authorizationHeader = authorizationHeader.Substring(7);
            }
            else
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();

            tokenHandler.ValidateToken(authorizationHeader, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Startup.JwtSecret))
            }, out var validatedToken);



            return true;
        }

        public AuthData GetAuthData(string id)
        {
            var expirationTime = DateTime.UtcNow.AddSeconds(jwtLifespan);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, id)
                }),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            return new AuthData
            {
                Token = token,
                TokenExpirationTime = ((DateTimeOffset)expirationTime).ToUnixTimeSeconds(),
                Id = id
            };
        }

        public string HashPassword(string password)
        {
            return password.GetHashCode().ToString();
            //return Crypto.HashPassword(password);
        }

        public bool VerifyPassword(string actualPassword, string hashedPassword)
        {
            return actualPassword.GetHashCode().ToString() == hashedPassword;
            //return Crypto.VerifyHashedPassword(hashedPassword, actualPassword);
        }
    }
    }
