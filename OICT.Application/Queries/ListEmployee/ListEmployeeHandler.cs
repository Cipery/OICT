using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OICT.Application.Dtos;
using OICT.Application.Queries.ListEmployees;
using OICT.Infrastructure.Repositories;

namespace OICT.Application.Queries.ListEmployee
{
    class ListEmployeeHandler : IRequestHandler<ListEmployeeQuery, IEnumerable<EmployeeModel>>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public ListEmployeeHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeModel>> Handle(ListEmployeeQuery request, CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.ListAsync();
            return _mapper.Map<List<EmployeeModel>>(employees);
        }
    }
}
