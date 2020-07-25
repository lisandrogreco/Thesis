using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Azure.WebJobs;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Legacy_Todo.Services;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Mvc;
using Azure_Todo_Functions.Data;
using System.Linq;

namespace Azure_Todo_Functions
{
    public class AuthFunction
    {

        IAuthService authService;
        readonly ICosmosDbService context;

        public AuthFunction(IAuthService authService, ICosmosDbService context)
        {
            this.authService = authService;            
            this.context = context;
        }

        [FunctionName("Login")]
        public async Task<IActionResult> Login([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<LoginRequest>(requestBody);

            var userResult = await context.GetUsersAsync($"SELECT * FROM c where c.Username = '{model.Username}'");
            var user = userResult.FirstOrDefault();
            
            if (user == null)
            {
                return new BadRequestObjectResult(new { email = "no user with this name" });
            }

            var passwordValid = authService.VerifyPassword(model.Password, user.Password);
            if (!passwordValid)
            {
                return new BadRequestObjectResult(new { password = "invalid password" });
            }

            return new OkObjectResult( authService.GetAuthData(user.id));
        }

        [FunctionName("Register")]
        public async Task<IActionResult> Register(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var model = JsonConvert.DeserializeObject<LoginRequest>(requestBody);

            var userResult = await context.GetUsersAsync($"SELECT * FROM c where c.Username = '{model.Username}'");
            var dBuser = userResult.FirstOrDefault();
            if (dBuser != null) return new BadRequestObjectResult(new { username = "user with this email already exists" });

            var id = Guid.NewGuid().ToString();
            var user = new User
            {
                id = id,
                Username = model.Username,
                Password = authService.HashPassword(model.Password)
            };
            try { 
             await context.AddUserAsync(user);
            } catch (Exception ex)
            {
                log.LogError(ex, "Exception!!!");
                return new BadRequestResult();
            }
            return new OkObjectResult(authService.GetAuthData(id));
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class User 
    {
        public string id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
