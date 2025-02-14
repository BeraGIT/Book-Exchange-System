using BLL.ManagerServices.Interfaces;
using DAL.Repos.Interfaces;
using ENTITIES.Models;

namespace BLL.ManagerServices.Concretes
{
    public class ListingManager : BaseManager<Listing>, IListingManager
    {
        IListingRepos _lisRep;

        public ListingManager(IListingRepos lisRep) : base(lisRep)
        {
            _lisRep = lisRep;
        }
    }
}
