using MediatR;
using OICT.Application.Dtos;

namespace OICT.Application.Commands.CreateEmployee
{
    public class CreateEmployeeCommand : IRequest<int>
    {
        public CreateEmployeeModel Model { get; }

        public CreateEmployeeCommand(CreateEmployeeModel model)
        {
            Model = model;
        }
    }
}
