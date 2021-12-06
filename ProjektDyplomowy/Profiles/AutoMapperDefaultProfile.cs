using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjektDyplomowy.Entities;

namespace ProjektDyplomowy.Profiles
{
    public class AutoMapperDefaultProfile : Profile
    {
        public AutoMapperDefaultProfile()
        {
            CreateMap<Category, SelectListItem>()
                .ForMember(des => des.Text, opt => opt.MapFrom(src => src.Name))
                .ForMember(des => des.Value, opt => opt.MapFrom(src => src.Id));
        }
    }
}
