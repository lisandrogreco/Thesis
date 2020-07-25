using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Legacy_Todo.Models;
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

        Task<User> GetUser(string username);

        Task AddUser(User user);
    }

    public class AuthService : IAuthService
    {
        string jwtSecret;
        int jwtLifespan;
        AmazonDynamoDBClient client;

        public AuthService(string jwtSecret, int jwtLifespan)
        {
            this.jwtSecret = jwtSecret;
            this.jwtLifespan = jwtLifespan;
            var credentials = new BasicAWSCredentials("REMOVE_ACCESS_KEY", "REMOVE_SECRET_KEY");
            client = new AmazonDynamoDBClient(credentials);
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

        public async Task<User> GetUser(string username)
        {
            var ctx = new DynamoDBContext(client);
            var result = await ctx.ScanAsync<User>(new List<ScanCondition> { new ScanCondition("Username", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, username) }).GetRemainingAsync();
            return result.FirstOrDefault();
        }

        public async Task AddUser(User user)
        {
            var ctx = new DynamoDBContext(client);
            await ctx.SaveAsync(user);
            return;
        }
       
    }
}
