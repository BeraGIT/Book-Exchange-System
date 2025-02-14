using DAL.Context;
using DAL.Repos.Interfaces;
using ENTITIES.Models;
using Microsoft.AspNetCore.Identity;

namespace DAL.Repos.Concretes
{
    public class UserRepos : BaseRepository<User>, IUserRepos
    {
        

        public UserRepos(MyContext db, UserManager<User> userManager) : base(db)
        {
            
        }
        public async Task<bool> CreateAsync(User user, string password)
        {
            throw new NotImplementedException();
        }
    }
}
