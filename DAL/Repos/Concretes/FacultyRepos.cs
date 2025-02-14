using DAL.Context;
using DAL.Repos.Interfaces;
using ENTITIES.Models;

namespace DAL.Repos.Concretes
{
    public class FacultyRepos : BaseRepository<Faculty>, IFacultyRepos
    {
        public FacultyRepos(MyContext db) : base(db)
        {

        }
    }
}
