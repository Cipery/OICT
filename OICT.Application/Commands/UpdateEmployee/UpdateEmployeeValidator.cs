using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace OICT.Application.Commands.UpdateEmployee
{
    class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeValidator()
        {
            RuleFor(x => x.UpdateEmployeeModel).NotNull();
            RuleFor(x => x.UpdateEmployeeModel.ChildrenCount).InclusiveBetween(0, 15);
            RuleFor(x => x.UpdateEmployeeModel.DateOfBirth).NotEqual(default(DateTime));
            RuleFor(x => x.UpdateEmployeeModel.StartOfEmployment).NotEqual(default(DateTime));
        }
    }
}
