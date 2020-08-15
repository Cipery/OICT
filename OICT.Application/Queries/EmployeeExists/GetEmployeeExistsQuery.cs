using MediatR;

namespace OICT.Application.Queries.EmployeeExists
{
    public class GetEmployeeExistsQuery : IRequest<bool>
    {
        public int Id { get; }

        public GetEmployeeExistsQuery(int id)
        {
            Id = id;
        }
    }
}
