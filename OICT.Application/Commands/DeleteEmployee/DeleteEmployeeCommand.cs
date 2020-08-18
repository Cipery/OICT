using MediatR;
using OICT.Application.Dtos;

namespace OICT.Application.Commands.DeleteEmployee
{
    public class DeleteEmployeeCommand : IRequest<EmployeeModel>
    {
        public int Id { get; }

        public DeleteEmployeeCommand(int id)
        {
            Id = id;
        }
    }
}