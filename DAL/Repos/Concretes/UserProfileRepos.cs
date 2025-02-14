using DAL.Context;
using DAL.Repos.Interfaces;
using ENTITIES.Models;

namespace DAL.Repos.Concretes
{
    public class UserProfileRepos : BaseRepository<UserProfile>, IUserProfileRepos
    {
        public UserProfileRepos(MyContext db) : base(db)
        {

        }
    }
}
