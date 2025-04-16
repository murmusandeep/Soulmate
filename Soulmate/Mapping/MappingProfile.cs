using AutoMapper;
using Shared.Models;
using SoulmateDAL.Entities;

namespace Soulmate.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, User>();
        }
    }
}
