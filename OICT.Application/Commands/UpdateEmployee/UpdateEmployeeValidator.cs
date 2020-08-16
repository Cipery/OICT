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
        }
    }
}
