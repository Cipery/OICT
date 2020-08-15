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
            RuleFor(x => x.Username).NotNull();
            RuleFor(x => x.Password).NotNull();
        }
    }
}
