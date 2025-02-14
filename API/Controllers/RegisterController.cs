using API.Models.Users.RequestModel;
using ENTITIES.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<User> _identityUserManager;

        public RegisterController(UserManager<User> identityUserManager)
        {
            _identityUserManager = identityUserManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys
                    .SelectMany(key => ModelState[key].Errors.Select(x => new
                    {
                        field = key,
                        message = x.ErrorMessage
                    })).ToList();

                return BadRequest(new { success = false, errors });
            }

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                CreatedDate = DateTime.Now
            };

            var identityResult = await _identityUserManager.CreateAsync(user, request.Password);

            if (!identityResult.Succeeded)
            {
                var identityErrors = identityResult.Errors.Select(e => new
                {
                    field = "General",
                    message = e.Description
                });

                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, errors = identityErrors });
            }

            return Ok(new { success = true, message = "Kullanıcı başarıyla kaydedildi!" });
        }

    }
}

