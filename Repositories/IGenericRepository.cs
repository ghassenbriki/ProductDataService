using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Leoni.Repositories
{
    public interface IGenericRepository <T> where T : class
    {
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        EntityEntry Attach(T entity);

        void Update(T entity, params string[] properties);

        IQueryable<T> Filter(
    Expression<Func<T, bool>> predicate,
    Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
    params Expression<Func<T, object>>[] includes);

        Task SaveAsync();


    }
}
