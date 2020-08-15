using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace OICT.Application.Queries.VerifiyCredentials
{
    public class VerifyCredentialsQuery : IRequest<bool>
    {
        public string Username { get; }
        public string Password { get; }

        public VerifyCredentialsQuery(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
