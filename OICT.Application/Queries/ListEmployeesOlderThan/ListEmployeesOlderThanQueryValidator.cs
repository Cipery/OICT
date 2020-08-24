using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace OICT.Application.Queries.ListEmployeesOlderThan
{
    class ListEmployeesOlderThanQueryValidator : AbstractValidator<ListEmployeesOlderThanQuery>
    {
        public ListEmployeesOlderThanQueryValidator()
        {
            RuleFor(x => x.Age).InclusiveBetween(0, 125);
        }
    }
}
