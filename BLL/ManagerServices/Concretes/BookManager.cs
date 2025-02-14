using BLL.ManagerServices.Interfaces;
using DAL.Repos.Interfaces;
using ENTITIES.Models;

namespace BLL.ManagerServices.Concretes
{
    public class BookManager : BaseManager<Book>, IBookManager
    {
        IBookRepos _bkRep;

        public BookManager(IBookRepos bkRep) : base(bkRep)
        {
            _bkRep = bkRep;
        }
       


    }
}
