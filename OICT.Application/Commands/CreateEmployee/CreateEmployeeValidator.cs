using System;
using FluentValidation;

namespace OICT.Application.Commands.CreateEmployee
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.Model)
                .NotNull()
                .WithMessage("Model is null");
            RuleFor(x => x.Model.Name).NotNull().NotEmpty();
            RuleFor(x => x.Model.ChildrenCount).InclusiveBetween(0, 15);
            RuleFor(x => x.Model.DateOfBirth).NotEqual(default(DateTime));
            RuleFor(x => x.Model.StartOfEmployment).NotEqual(default(DateTime));
        }
    }
}
