using Amazon.Lambda.Core;
using Legacy_Todo.Models;
using Legacy_Todo.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Legacy_Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskAuthController : ControllerBase
    {
        IAuthService authService;
     
        public TaskAuthController(IAuthService authService)
        {
            this.authService = authService;
        }


        [HttpPost]
        public async Task<ActionResult<AuthData>> Post(LoginRequest model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string id;

            if (model.IsLogin)
            {
                var user = await authService.GetUser(model.Username);

                if (user == null)
                {
                    return BadRequest(new { email = "no user with this name" });
                }

                var passwordValid = authService.VerifyPassword(model.Password, user.Password);
                if (!passwordValid)
                {
                    return BadRequest(new { password = "invalid password" });
                }
                id = user.id;
            }
            else
            {
                try
                {
                    var usernameUniq = await authService.GetUser(model.Username);

                    if (usernameUniq != null) return BadRequest(new { username = "user with this email already exists" });

                    id = Guid.NewGuid().ToString();
                    var user = new User
                    {
                        id = id,
                        Username = model.Username,
                        Password = authService.HashPassword(model.Password)
                    };
                    await authService.AddUser(user);
                    

                    return authService.GetAuthData(id);
                }
                catch (Exception ex)
                {
                    LambdaLogger.Log($"ERROR: {ex.Message}");
                    return null;
                }
            }
            return authService.GetAuthData(id);
        }
    }
}