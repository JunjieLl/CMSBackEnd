using AutoMapper;
using CMS.Models;

namespace CMS.Mapper;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, LoginOutDto>();
    }
}


