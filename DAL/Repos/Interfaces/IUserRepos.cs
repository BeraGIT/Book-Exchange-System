using ENTITIES.Models;

namespace DAL.Repos.Interfaces
{
    public interface IUserRepos : IRepository<User>
    {
        Task<bool> CreateAsync(User user, string password);
    }
}
