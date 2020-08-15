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

            RuleFor(x => x.Model.Age)
                .GreaterThanOrEqualTo(15)
                .WithMessage("Age cannot be lower than 15 years old");
        }
    }
}
