using System.Linq.Expressions;

namespace EHRLite.Repository
{
    public interface IRepository<T> where T : class
    {
        // Sadece tanımlama yapıyoruz, kod bloğu yok!
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}