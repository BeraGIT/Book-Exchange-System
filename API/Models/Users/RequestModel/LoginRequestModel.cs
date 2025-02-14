using System.ComponentModel.DataAnnotations;

namespace API.Models.Users.RequestModel
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "E-posta zorunludur!")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur!")]
        public string Password { get; set; }
    }

}
