using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace OICT.Application.Queries.VerifiyCredentials
{
    class VerifyCredentialsQueryHandler : IRequestHandler<VerifyCredentialsQuery, bool>
    {
        public Task<bool> Handle(VerifyCredentialsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(request.Username.Equals("admin") && request.Password.Equals("123456"));
        }
    }
}
