using API.LoginCtrl;
using API.Models.Faculties.ResposeModels;
using API.Models.UserProfiles.RequestModels;
using API.Models.UserProfiles.ResponseModels;
using BLL.ManagerServices.Interfaces;
using ENTITIES.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileManager _userProfileManager;
        private readonly LoginStatus _loginStatus;
        private readonly IUserManager _userManager;
        private readonly IFacultyManager _facultyManager;

        public UserProfileController(
            IFacultyManager facultyManager,
            IUserProfileManager userProfileManager,
            LoginStatus loginStatus,
            IUserManager userManager)
        {
            _facultyManager = facultyManager;
            _userProfileManager = userProfileManager;
            _loginStatus = loginStatus;
            _userManager = userManager;
        }

        [HttpPost("CreateUserProfile")]
        public async Task<IActionResult> CreateUserProfile([FromBody] UserProfileCreateModel request)
        {
            // ModelState validasyonu
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

            // Kullanıcı oturum kontrolü
            if (!_loginStatus.IsLoggedIn)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Kullanıcı giriş yapmamış!"
                });
            }

            if (string.IsNullOrWhiteSpace(_loginStatus.LoggedInUserId))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Kullanıcı bilgisi bulunamadı!"
                });
            }

            // Kullanıcı doğrulaması
            var user = await _userManager.FirstOrDefaultAsync(u => u.Id == _loginStatus.LoggedInUserId);
            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Kullanıcı mevcut değil!"
                });
            }

            // Kullanıcının zaten bir profili var mı kontrol et
            if (user.Profile != null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Kullanıcının zaten bir profili mevcut!"
                });
            }

            // Fakülte doğrulaması
            if (!await _facultyManager.AnyAsync(x => x.ID == request.FacultyID))
            {
                return NotFound(new
                {
                    success = false,
                    message = "Fakülte bulunamadı!"
                });
            }

            // Profil oluşturma işlemi
            var userProfile = new UserProfile
            {
                Name = request.Name,
                Surname = request.Surname,
                FacultyID = request.FacultyID,
                UserId = _loginStatus.LoggedInUserId
            };

            await _userProfileManager.AddAsync(userProfile);

            return Ok(new
            {
                success = true,
                message = "Kullanıcı profili başarıyla oluşturuldu!",
                data = new
                {
                    ProfileId = userProfile.ID,
                    Name = userProfile.Name,
                    Surname = userProfile.Surname,
                    FacultyId = userProfile.FacultyID
                }
            });
        }


        [HttpGet("ProfileResponse")]
        public async Task<IActionResult> ProfileResponse()
        {
            // Kullanıcının giriş yapıp yapmadığını kontrol et
            if (!_loginStatus.IsLoggedIn)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Kullanıcı giriş yapmamış!"
                });
            }

            // Kullanıcı kimliği kontrolü
            var userId = _loginStatus.LoggedInUserId;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Kullanıcı bilgisi bulunamadı!"
                });
            }

            // Kullanıcıyı al
            var user = await _userManager.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || user.Profile == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Kullanıcı profili bulunamadı!"
                });
            }

            // Fakülte bilgilerini al
            var faculty = await _facultyManager.FirstOrDefaultAsync(f => f.ID == user.Profile.FacultyID);
            if (faculty == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Fakülte bilgisi bulunamadı!"
                });
            }

            // Yanıtı hazırlayın
            var response = new UserProfileResponseModel
            {
                UserID = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Name = user.Profile.Name,
                Surname = user.Profile.Surname,
                Faculty = new FacultyResponseModel
                {
                    ID = faculty.ID,
                    Name = faculty.FacultyName,
                    Address = faculty.FacultyAddress
                }
            };

            return Ok(new
            {
                success = true,
                message = "Kullanıcı profili başarıyla getirildi.",
                data = response
            });
        }

        [HttpPut("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileUpdateModel request)
        {
            // Kullanıcının giriş yapıp yapmadığını kontrol et
            if (!_loginStatus.IsLoggedIn)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Kullanıcı giriş yapmamış!"
                });
            }

            var userId = _loginStatus.LoggedInUserId;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Kullanıcı bilgisi bulunamadı!"
                });
            }

            // Kullanıcıyı al
            var user = await _userManager.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null || user.Profile == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Kullanıcı profili bulunamadı!"
                });
            }

            // Fakülte doğrulaması
            if (!await _facultyManager.AnyAsync(f => f.ID == request.FacultyID))
            {
                return NotFound(new
                {
                    success = false,
                    message = "Fakülte bilgisi bulunamadı!"
                });
            }

            // Güncelleme işlemi
            user.Profile.Name = request.Name;
            user.Profile.Surname = request.Surname;
            user.Profile.FacultyID = request.FacultyID;

            await _userProfileManager.UpdateAsync(user.Profile);

            return Ok(new
            {
                success = true,
                message = "Kullanıcı profili başarıyla güncellendi!"
            });
        }

    }
}



