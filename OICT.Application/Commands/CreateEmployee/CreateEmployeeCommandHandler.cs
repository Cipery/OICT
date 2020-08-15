using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OICT.Domain.Model;
using OICT.Infrastructure.Common;
using OICT.Infrastructure.Repositories;

namespace OICT.Application.Commands.CreateEmployee
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public CreateEmployeeCommandHandler(IUnitOfWork unitOfWork, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<EmployeeEntity>(request.Model);
            _employeeRepository.Add(entity);
            await _unitOfWork.CommitAsync();
            return entity.Id;
        }
    }
}
