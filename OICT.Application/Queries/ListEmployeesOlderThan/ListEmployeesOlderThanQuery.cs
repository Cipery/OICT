using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using OICT.Application.Dtos;

namespace OICT.Application.Queries.ListEmployeeOlderThan
{
    public class ListEmployeesOlderThanQuery : IRequest<IEnumerable<EmployeeModel>>
    {
        public int Age { get; }

        public ListEmployeesOlderThanQuery(int age)
        {
            Age = age;
        }
    }
}
