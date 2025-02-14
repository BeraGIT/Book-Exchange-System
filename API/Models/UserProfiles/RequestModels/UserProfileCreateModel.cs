using System.ComponentModel.DataAnnotations;

namespace API.Models.UserProfiles.RequestModels
{
    public class UserProfileCreateModel
    {
        [Required(ErrorMessage = "Ad alanı zorunludur!")]
        [MaxLength(50, ErrorMessage = "Ad en fazla 50 karakter olmalıdır!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur!")]
        [MaxLength(50, ErrorMessage = "Soyad en fazla 50 karakter olmalıdır!")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Fakülte ID zorunludur!")]
        public string FacultyID { get; set; }
    }
}
