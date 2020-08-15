using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OICT.Infrastructure.Common
{
    public interface IUnitOfWork : IDisposable
    {
        OICTApiContext Context { get; set; }
        Task CommitAsync();
    }
}
