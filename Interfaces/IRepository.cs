using System.Linq.Expressions;

public interface IRepository<T>
{
    Task<T> GetByIdAsync(int id, Enum[] includes);
    Task<IEnumerable<T>> GetAllAsync(Enum[] includes);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task RemoveAsync(T entity);
}
