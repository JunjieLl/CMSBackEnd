using AutoMapper;

namespace CMS.Mapper;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, LoginDto>();
    }
}


