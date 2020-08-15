using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using OICT.Application.Queries.ListEmployeeOlderThan;

namespace OICT.Application.Queries.ListEmployeesOlderThan
{
    class ListEmployeesOlderThanQueryValidator : AbstractValidator<ListEmployeesOlderThanQuery>
    {
        public ListEmployeesOlderThanQueryValidator()
        {
            RuleFor(x => x.Age).Must(age => age >= 0 && age <= 125)
                .WithMessage("Age must be between 0 and 125");
        }
    }
}
