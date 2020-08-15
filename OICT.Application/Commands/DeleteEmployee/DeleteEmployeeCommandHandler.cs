using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OICT.Application.Dtos;
using OICT.Infrastructure.Common;
using OICT.Infrastructure.Repositories;

namespace OICT.Application.Commands.DeleteEmployee
{
    class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, EmployeeModel>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteEmployeeCommandHandler(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeModel> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _employeeRepository.FindAsync(request.Id);

            if (entity == null)
            {
                return null;
            }

            _employeeRepository.Delete(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<EmployeeModel>(entity);
        }
    }
}
