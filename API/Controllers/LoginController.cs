using API.LoginCtrl;
using API.Models.Users.RequestModel;
using BLL.ManagerServices.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginStatus _loginStatus;
        private readonly IUserManager _userManager;

        public LoginController(LoginStatus loginStatus, IUserManager userManager)
        {
            _loginStatus = loginStatus;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
        {
            // Kullanıcı giriş yapmışsa
            if (_loginStatus.IsLoggedIn)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Kullanıcı zaten giriş yapmış durumda!"
                });
            }

            // ModelState üzerinden validasyon kontrolü
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Keys
                    .SelectMany(key => ModelState[key].Errors.Select(error => new
                    {
                        field = key,
                        message = error.ErrorMessage
                    })).ToList();

                return BadRequest(new { success = false, errors });
            }

            // Kullanıcı doğrulama işlemi
            var result = await _userManager.ValidateUserAsync(request.Email, request.Password);

            if (!result.IsSuccess)
            {
                return Unauthorized(new
                {
                    success = false,
                    errors = new[]
                    {
                new { field = "General", message = result.Message }
            }
                });
            }

            // Kullanıcı doğrulandıysa, oturum durumu güncellenir
            _loginStatus.IsLoggedIn = true;
            _loginStatus.LoggedInUserId = result.User.Id;

            // Başarı durumunda dönüş
            return Ok(new
            {
                success = true,
                message = "Giriş başarılı.",
                data = new { UserId = result.User.Id }
            });
        }


    }
}








