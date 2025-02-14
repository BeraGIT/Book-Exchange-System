using DAL.Context;
using DAL.Repos.Interfaces;
using ENTITIES.Models;

namespace DAL.Repos.Concretes
{
    public class BookRepos : BaseRepository<Book>, IBookRepos
    {
        public BookRepos(MyContext db) : base(db)
        {

        }
    }
}
