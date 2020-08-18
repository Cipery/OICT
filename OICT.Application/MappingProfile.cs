using AutoMapper;
using OICT.Application.Dtos;
using OICT.Domain.Model;

namespace OICT.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateEmployeeModel, EmployeeEntity>()
                .ForMember(entity => entity.Id, opts => opts.Ignore());
            CreateMap<EmployeeEntity, EmployeeModel>();
            CreateMap<UpdateEmployeeModel, EmployeeEntity>();
        }
    }
}
