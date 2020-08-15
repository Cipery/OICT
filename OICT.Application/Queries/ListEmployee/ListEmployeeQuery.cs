using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using OICT.Application.Dtos;

namespace OICT.Application.Queries.ListEmployees
{
    public class ListEmployeeQuery : IRequest<IEnumerable<EmployeeModel>>
    {
    }
}
