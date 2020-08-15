using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OICT.Application.Dtos;
using OICT.Infrastructure.Repositories;

namespace OICT.Application.Commands.UpdateEmployee
{
    public class UpdateEmployeeCommand : IRequest
    {
        public UpdateEmployeeModel UpdateEmployeeModel { get; }

        public UpdateEmployeeCommand(UpdateEmployeeModel updateEmployeeModel)
        {
            UpdateEmployeeModel = updateEmployeeModel;
        }

        
    }
}
