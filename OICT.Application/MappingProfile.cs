using AutoMapper;
using OICT.Application.Dtos;
using OICT.Domain.Model;

namespace OICT.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateEmployeeModel, EmployeeEntity>();
            CreateMap<EmployeeEntity, EmployeeModel>();
            CreateMap<UpdateEmployeeModel, EmployeeEntity>();
        }
    }
}
