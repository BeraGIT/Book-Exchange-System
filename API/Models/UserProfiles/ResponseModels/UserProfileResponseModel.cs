using API.Models.Faculties.ResposeModels;

namespace API.Models.UserProfiles.ResponseModels
{
    public class UserProfileResponseModel
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public string Email  { get; set;  }
        public string Name { get; set; }
        public string Surname { get; set; }
        public FacultyResponseModel Faculty { get; set; }

    }
}
