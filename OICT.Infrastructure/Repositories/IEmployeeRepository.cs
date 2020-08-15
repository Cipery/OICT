using System.Collections.Generic;
using System.Threading.Tasks;
using OICT.Domain.Model;
using OICT.Infrastructure.Common;

namespace OICT.Infrastructure.Repositories
{
    public interface IEmployeeRepository : IRepository<EmployeeEntity>
    {
        Task<List<EmployeeEntity>> ListEmployeesOlderThanAsync(int age);
    }
}
