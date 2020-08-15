using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OICT.Application.Dtos;
using OICT.Infrastructure.Repositories;

namespace OICT.Application.Queries.GetEmployee
{
    public class GetEmployeeQuery : IRequest<EmployeeModel>
    {
        public int Id { get; }

        public GetEmployeeQuery(int id)
        {
            Id = id;
        }
    }
}
