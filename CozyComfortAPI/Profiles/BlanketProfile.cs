using AutoMapper;
using CozyComfortAPI.Model;
using CozyComfortAPI.DTO;
namespace CozyComfortAPI.Profiles
{
    public class BlanketProfile:Profile
    {
        public BlanketProfile()
        {
            CreateMap<ModelWriteDTO, BlanketModel>();
            CreateMap<BlanketModel, ModelReadDTO>()
    .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
    .ForMember(dest => dest.MaterialDescription, opt => opt.MapFrom(src => src.Material.Description));
        }
    }
}
