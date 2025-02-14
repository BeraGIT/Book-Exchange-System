using System.ComponentModel.DataAnnotations;

namespace API.Models.Users.RequestModel
{
    public class RegisterRequestModel
    {
        [Required(ErrorMessage = "E-posta alanı zorunludur!")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur!")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre onayı zorunludur!")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor!")]
        public string ConfirmedPassword { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur!")]
        public string UserName { get; set; }
    }
}
