using ENTITIES.Models;

namespace BLL.ManagerServices.Interfaces
{
    public interface IUserManager : IManager<User>
    {
        Task<bool> CreateAsync(User user, string password);
     
        //Derived
        Task<(bool IsSuccess, string Message, User User)> ValidateUserAsync(string email, string password);


    }
}
