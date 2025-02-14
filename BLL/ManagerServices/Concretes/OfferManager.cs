using BLL.ManagerServices.Interfaces;
using DAL.Repos.Interfaces;
using ENTITIES.Models;

namespace BLL.ManagerServices.Concretes
{
    public class OfferManager : BaseManager<Offer>, IOfferManager
    {
        IOfferRepos _offRep;

        public OfferManager(IOfferRepos offRep) : base(offRep)
        {
            _offRep = offRep;
        }
    }
}
