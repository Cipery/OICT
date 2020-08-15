using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OICT.Application.Dtos;
using OICT.Infrastructure.Repositories;

namespace OICT.Application.Queries.ListEmployeeOlderThan
{
    class ListEmployeesOlderThanQueryHandler : IRequestHandler<ListEmployeesOlderThanQuery, IEnumerable<EmployeeModel>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public ListEmployeesOlderThanQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeModel>> Handle(ListEmployeesOlderThanQuery request, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.ListEmployeesOlderThanAsync(request.Age);
            return _mapper.Map<List<EmployeeModel>>(employees);
        }
    }
}
