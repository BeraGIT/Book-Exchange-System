using System.Linq.Expressions;
using DAL.Context;
using DAL.Repos.Interfaces;
using ENTITIES.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repos.Concretes
{
    public class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly MyContext _context;

        public BaseRepository(MyContext context)
        {
            _context = context;
        }

        // List Commands
        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsQueryable().AsNoTracking();
        }

        public IQueryable<T> GetActives()
        {
            return Where(entity => entity.Status != ENTITIES.Enums.DataStatus.Deleted);
        }

        public IQueryable<T> GetModifieds()
        {
            return Where(entity => entity.Status == ENTITIES.Enums.DataStatus.Updated);
        }

        public IQueryable<T> GetPassives()
        {
            return Where(entity => entity.Status == ENTITIES.Enums.DataStatus.Deleted);
        }

        // Modify Commands
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            entity.Status = ENTITIES.Enums.DataStatus.Updated;
            entity.UpdatedDate = DateTime.UtcNow;

            T original = await EnsureExistsAsync(entity.ID);

            _context.Entry(original).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        private async Task<T> EnsureExistsAsync(string id)
        {
            var entity = await FindAsync(id);

            if (entity == null)
            {
                throw new InvalidOperationException($"Entity with ID '{id}' not found.");
            }

            return entity;
        }

        public async Task UpdateRangeAsync(List<T> entities)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity);
            }
        }

        public async Task DeleteAsync(T entity)
        {
            entity.Status = ENTITIES.Enums.DataStatus.Deleted;
            entity.DeletedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(List<T> entities)
        {
            foreach (var entity in entities)
            {
                await DeleteAsync(entity);
            }
        }

        public void Destroy(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public void DestroyRange(List<T> entities)
        {
            foreach (var entity in entities)
            {
                Destroy(entity);
            }
        }

        // Linq Commands
        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().AsNoTracking().Where(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AsNoTracking().AnyAsync(predicate);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public IQueryable<X> SelectAsync<X>(Expression<Func<T, X>> selector)
        {
            return _context.Set<T>().Select(selector);
        }

        // Find
        public async Task<T> FindAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
    }
}


