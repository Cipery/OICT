using System.Collections.Generic;
using MediatR;
using OICT.Application.Dtos;

namespace OICT.Application.Queries.ListEmployeesOlderThan
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
