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
        }
    }
}
