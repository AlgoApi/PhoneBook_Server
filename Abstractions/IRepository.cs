using System.Linq.Expressions;

namespace PhoneBook.Abstractions
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetSearchAsync(Expression<Func<T, bool>> condition);
        Task<T?> GetAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
    }
}
