using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OICT.Infrastructure.Common
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly IUnitOfWork _unitOfWork;
        protected OICTApiContext Context => _unitOfWork.Context;

        protected Repository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<T> List()
        {
            return Context.Set<T>().ToList<T>();
        }

        public async Task<List<T>> ListAsync()
        {
            return await Context.Set<T>().ToListAsync();
        }

        public T Add(T entity)
        {
            Context.Set<T>().Add(entity);
            return entity;
        }

        public void AddRange(IEnumerable<T> entities)
        {
            Context.Set<T>().AddRange(entities);
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public ValueTask<T> FindAsync(params object[] keys)
        {
            return Context.FindAsync<T>(keys);
        }

        public async Task<bool> ExistsAsync(params object[] keys)
        {
            return (await FindAsync(keys)) != null;
        }

        public T Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
