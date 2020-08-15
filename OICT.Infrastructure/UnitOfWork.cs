using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OICT.Infrastructure.Common;

namespace OICT.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        public OICTApiContext Context { get; set; }

        public UnitOfWork(OICTApiContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CommitAsync()
        {
            await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
