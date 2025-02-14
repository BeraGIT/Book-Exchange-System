using System.Linq.Expressions;
using BLL.ManagerServices.Interfaces;
using DAL.Repos.Interfaces;
using ENTITIES.Interfaces;


namespace BLL.ManagerServices.Concretes
{
    public class BaseManager<T> : IManager<T> where T : class, IEntity
    {
        protected readonly IRepository<T> _repository;
        

        public BaseManager(IRepository<T> repository )
        {
            _repository = repository ;
           

        }
       
        // List Commands
        public IQueryable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public IQueryable<T> GetActives()
        {
            return _repository.GetActives();
        }

        public IQueryable<T> GetModifieds()
        {
            return _repository.GetModifieds();
        }

        public IQueryable<T> GetPassives()
        {
            return _repository.GetPassives();
        }

        // Modify Commands
        
        public async Task AddAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            else
            {
                await _repository.AddAsync(entity);
            }
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            if (entities == null || !entities.Any()) throw new ArgumentNullException(nameof(entities));
            await _repository.AddRangeAsync(entities);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _repository.UpdateAsync(entity);
        }

        public async Task UpdateRangeAsync(List<T> entities)
        {
            if (entities == null || !entities.Any()) throw new ArgumentNullException(nameof(entities));
            await _repository.UpdateRangeAsync(entities);
        }

        public async Task DeleteAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _repository.DeleteAsync(entity);
        }

        public async Task DeleteRangeAsync(List<T> entities)
        {
            if (entities == null || !entities.Any()) throw new ArgumentNullException(nameof(entities));
            await _repository.DeleteRangeAsync(entities);
        }

        public void Destroy(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _repository.Destroy(entity);
        }

        public void DestroyRange(List<T> entities)
        {
            if (entities == null || !entities.Any()) throw new ArgumentNullException(nameof(entities));
            _repository.DestroyRange(entities);
        }

        // Linq Commands
        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return _repository.Where(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await _repository.AnyAsync(predicate);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return await _repository.FirstOrDefaultAsync(predicate);
        }

        public IQueryable<X> SelectAsync<X>(Expression<Func<T, X>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return _repository.SelectAsync(selector);
        }

        
        public async Task<T> FindAsync(string id)
        {
            return await _repository.FindAsync(id);
        }

        //Derrived

       
          







    }
}

