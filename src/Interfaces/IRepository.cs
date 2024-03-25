using System.Linq.Expressions;

namespace iBudget.Interfaces;

public interface IRepository<T>
{
    Task<T> GetByIdAsync(int id, Enum[] includes);
    IQueryable<T> GetAll(Enum[] includes);
    IQueryable<T> Find(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task RemoveAsync(T entity);
}