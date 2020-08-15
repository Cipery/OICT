using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OICT.Application.Dtos;
using OICT.Infrastructure.Repositories;

namespace OICT.Application.Queries.GetEmployee
{
    public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, EmployeeModel>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public GetEmployeeQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<EmployeeModel> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {
            var entity = await _employeeRepository.FindAsync(request.Id);
            return _mapper.Map<EmployeeModel>(entity);
        }
    }
}
