using System.Linq.Expressions;
using ENTITIES.Interfaces;
using ENTITIES.Models;

namespace BLL.ManagerServices.Interfaces
{
    public interface IManager<T> where T : class, IEntity
    {
        // List Commands
        IQueryable<T> GetAll();
        IQueryable<T> GetActives();
        IQueryable<T> GetModifieds();
        IQueryable<T> GetPassives();

        // Modify Commands

        Task AddAsync(T entity);
        Task AddRangeAsync(List<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(List<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(List<T> entities);
        Task<T> FindAsync(string id);
        void Destroy(T entity);
        void DestroyRange(List<T> entities);


        // Linq Commands
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        IQueryable<X> SelectAsync<X>(Expression<Func<T, X>> selector);

        //Derrived Commands
      





    }
}

