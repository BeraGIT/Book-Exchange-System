using BLL.ManagerServices.Interfaces;
using DAL.Repos.Interfaces;
using ENTITIES.Models;

namespace BLL.ManagerServices.Concretes
{
    public class FacultyManager : BaseManager<Faculty>, IFacultyManager
    {
        IFacultyRepos _faRep;

        public FacultyManager(IFacultyRepos faRep) : base(faRep)
        {
            _faRep = faRep;
        }
    }
}
