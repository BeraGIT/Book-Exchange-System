using DAL.Context;
using DAL.Repos.Interfaces;
using ENTITIES.Models;

namespace DAL.Repos.Concretes
{
    public class ListingRepos : BaseRepository<Listing>, IListingRepos
    {
        public ListingRepos(MyContext db) : base(db)
        {

        }
    }
}
