using Microsoft.EntityFrameworkCore;
using OICT.Domain.Model;

namespace OICT.Infrastructure
{
    public class OICTApiContext : DbContext
    {
        public OICTApiContext()
        {
        }

        public OICTApiContext (DbContextOptions<OICTApiContext> options)
            : base(options)
        {
        }

        public DbSet<EmployeeEntity> Employee { get; set; }
    }
}
