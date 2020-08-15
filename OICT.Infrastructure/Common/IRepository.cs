using System.Collections.Generic;
using System.Threading.Tasks;

namespace OICT.Infrastructure.Common
{
    public interface IRepository<T> where T : class
    {
        List<T> List();
        Task<List<T>> ListAsync();
        T Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Delete(T entity);
        ValueTask<T> FindAsync(params object[] keys);
        Task<bool> ExistsAsync(params object[] keys);
    }
}
