namespace API.LoginCtrl
{
    public class LoginStatus
    {
        public bool IsLoggedIn { get; set; } = false; 
        public string? LoggedInUserId { get; set; } = null;     
    }
}
