using DAL.Context;
using DAL.Repos.Interfaces;
using ENTITIES.Models;

namespace DAL.Repos.Concretes
{
    public class OfferRepos : BaseRepository<Offer>, IOfferRepos
    {
        public OfferRepos(MyContext db) : base(db)
        {

        }
    }
}
