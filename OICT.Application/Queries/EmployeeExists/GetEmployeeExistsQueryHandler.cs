using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OICT.Infrastructure.Repositories;

namespace OICT.Application.Queries.EmployeeExists
{
    class GetEmployeeExistsQueryHandler : IRequestHandler<GetEmployeeExistsQuery, bool>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeeExistsQueryHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<bool> Handle(GetEmployeeExistsQuery request, CancellationToken cancellationToken)
        {
            return await _employeeRepository.ExistsAsync(request.Id);
        }
    }
}
