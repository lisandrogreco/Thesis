using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Legacy_Todo.Models;
using Legacy_Todo.Repos;
using Legacy_Todo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Legacy_Todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthService authService;
        IUserRepository userRepository;
        private static List<User> _testUsers = new List<User>() { new User { Password = "test", Username = "test" } };


        public AuthController(IAuthService authService, IUserRepository userRepository)
        {
            this.authService = authService;
            this.userRepository = userRepository;
        }

        /* [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            //var context = await _interaction.GetAuthorizationContextAsync(request.returnUrl);
           var user = _testUsers.FirstOrDefault(usr => usr.Password == request.password && usr.Username == request.username);

            if (user != null && context != null)
            {
                await HttpContext.SignInAsync(user.SubjectId, user.Username);
                return new JsonResult(new { RedirectUrl = request.returnUrl, IsOk = true });
            }

            return Unauthorized();
        }*/

        [HttpPost("login")]
        public ActionResult<AuthData> Post([FromBody]LoginRequest model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = userRepository.GetSingle(u => u.Username == model.Username);

            if (user == null)
            {
                return BadRequest(new { email = "no user with this name" });
            }

            var passwordValid = authService.VerifyPassword(model.Password, user.Password);
            if (!passwordValid)
            {
                return BadRequest(new { password = "invalid password" });
            }

            return authService.GetAuthData(user.Id);
        }

        [HttpPost("register")]
        public ActionResult<AuthData> Register([FromBody]LoginRequest model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

           
            var usernameUniq = userRepository.IsUsernameUniq(model.Username);
            if (!usernameUniq) return BadRequest(new { username = "user with this email already exists" });

            var id = Guid.NewGuid().ToString();
            var user = new User
            {
                Id = id,
                Username = model.Username,                
                Password = authService.HashPassword(model.Password)
            };
            userRepository.Add(user);
            userRepository.Commit();

            return authService.GetAuthData(id);
        }

    }

   
    }