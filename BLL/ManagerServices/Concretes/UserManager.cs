using BLL.ManagerServices.Interfaces;
using DAL.Repos.Interfaces;
using ENTITIES.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.ManagerServices.Concretes
{
    public class UserManager : BaseManager<User>, IUserManager
    {
        private readonly IUserRepos _userRep;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _identityUserManager;

        public UserManager(IUserRepos userRep, Microsoft.AspNetCore.Identity.UserManager<User> identityUserManager) : base(userRep)
        {
            _userRep = userRep;
            _identityUserManager = identityUserManager;
        }

        public async Task<bool> CreateAsync(User user, string password)
        {
            return await _userRep.CreateAsync(user, password);
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
            return await _identityUserManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _identityUserManager.CheckPasswordAsync(user, password);
        }

       
        
        public async Task<(bool IsSuccess, string Message, User User)> ValidateUserAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return (false, "Tüm alanlar doldurulmalıdır.", null);
            }

            var user = await FindUserByEmailAsync(email);

            if (user == null || !await CheckPasswordAsync(user, password))
            {
                return (false, "E-posta veya şifre yanlış.", null);
            }

            return (true, "Başarılı giriş.", user);
        }
    }
}
