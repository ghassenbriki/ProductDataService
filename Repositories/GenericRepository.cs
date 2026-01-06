using Leoni.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Leoni.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class 
    {
        private readonly LeoniDbContext _context;
        public GenericRepository(LeoniDbContext context)
        {
            _context = context;
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Remove(T entity)
        {
             _context.Set<T>().Remove(entity);
        }

        public DbSet<T> GetDbSet()
        {
            return _context.Set<T>();
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public EntityEntry Attach(T entity)
        {
            return _context.Set<T>().Attach(entity);
        }

        public void Update(T entity, params string[] properties)
        {
            var entry = Attach(entity);

            if (properties != null && properties.Length > 0)
            {
                foreach (var propertyName in properties)
                {
                    entry.Property(propertyName).IsModified = true;
                }
            }
            else
            {
                entry.State = EntityState.Modified; // update all columns
            }
        }


        public IQueryable<T> Filter(
    Expression<Func<T, bool>> predicate,
    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
    params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }







    }

}
