using API.LoginCtrl;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        private readonly LoginStatus _loginStatus;

        public LogoutController(LoginStatus loginStatus)
        {
            _loginStatus = loginStatus;
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Kullanıcının oturum durumunu kontrol et
            if (!_loginStatus.IsLoggedIn)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Oturum zaten açık değil!"
                });
            }

            // Oturumu kapat
            _loginStatus.IsLoggedIn = false;
            _loginStatus.LoggedInUserId = null;

            return Ok(new
            {
                success = true,
                message = "Oturum başarıyla kapatıldı!"
            });
        }

    }
}

