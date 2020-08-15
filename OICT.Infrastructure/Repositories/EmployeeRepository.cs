using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OICT.Domain.Model;
using OICT.Infrastructure.Common;

namespace OICT.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<EmployeeEntity>, IEmployeeRepository
    {
        private readonly IClock _clock;

        public EmployeeRepository(IUnitOfWork unitOfWork, IClock clock) : base(unitOfWork)
        {
            _clock = clock;
        }

        public async Task<List<EmployeeEntity>> ListEmployeesOlderThanAsync(int age)
        {
            var dt = _clock.UtcNow.AddYears(-age);
            return await Context.Employee.Where(entity => entity.DateOfBirth < dt).ToListAsync();
        }

    }
}
