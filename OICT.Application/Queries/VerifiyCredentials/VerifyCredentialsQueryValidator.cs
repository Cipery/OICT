using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace OICT.Application.Queries.VerifiyCredentials
{
    class VerifyCredentialsQueryValidator : AbstractValidator<VerifyCredentialsQuery>
    {
        public VerifyCredentialsQueryValidator()
        {
            RuleFor(x => x.AuthCredentials).NotNull();
            RuleFor(x => x.AuthCredentials.Username).NotNull().NotEmpty();
            RuleFor(x => x.AuthCredentials.Password).NotNull().NotEmpty();
        }
    }
}
