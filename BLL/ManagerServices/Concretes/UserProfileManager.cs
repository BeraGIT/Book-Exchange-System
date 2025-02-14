using BLL.ManagerServices.Interfaces;
using DAL.Repos.Interfaces;
using ENTITIES.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.ManagerServices.Concretes
{
    public class UserProfileManager : BaseManager<UserProfile>, IUserProfileManager
    {
        IUserProfileRepos _userpRep;

        public UserProfileManager(IUserProfileRepos userpRep) :     base(userpRep)
        {
            _userpRep = userpRep;
        }
       

    }
}
