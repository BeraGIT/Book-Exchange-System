using BLL.ManagerServices.Concretes;
using BLL.ManagerServices.Interfaces;
using DAL.Repos.Concretes;
using DAL.Repos.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.DependencyResolvers
{
    public static class RepositoryManagerServiceInjection
    {
        public static IServiceCollection AddRepManServices(this IServiceCollection services)
        {
            // Repository Services
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IBookRepos, BookRepos>();
            services.AddScoped<IFacultyRepos, FacultyRepos>();
            services.AddScoped<IListingRepos, ListingRepos>();
            services.AddScoped<IOfferRepos, OfferRepos>();
            services.AddScoped<IUserRepos, UserRepos>();
            services.AddScoped<IUserProfileRepos, UserProfileRepos>();

            // Manager Services
            services.AddScoped(typeof(IManager<>), typeof(BaseManager<>));
            services.AddScoped<IBookManager, BookManager>();
            services.AddScoped<IFacultyManager, FacultyManager>();
            services.AddScoped<IListingManager, ListingManager>();
            services.AddScoped<IOfferManager, OfferManager>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IUserProfileManager, UserProfileManager>();

            return services;
        }
    }
}
