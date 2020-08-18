using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using OICT.Application.Dtos;

namespace OICT.Application.Queries.VerifiyCredentials
{
    public class VerifyCredentialsQuery : IRequest<bool>
    {
        public AuthCredentialsModel AuthCredentials { get;}

        public VerifyCredentialsQuery(AuthCredentialsModel authCredentials)
        {
            AuthCredentials = authCredentials;
        }
    }
}
