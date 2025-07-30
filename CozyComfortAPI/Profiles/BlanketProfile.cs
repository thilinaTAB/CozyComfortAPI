using AutoMapper;
using CozyComfortAPI.Model;
using CozyComfortAPI.DTO;
namespace CozyComfortAPI.Profiles
{
    public class BlanketProfile:Profile
    {
        public BlanketProfile()
        {
            CreateMap<ModelWriteDTO, BlanketModel>()
                .ForMember(dest => dest.Materials, opt => opt.MapFrom(src => new Material
                {
                    MaterialName = src.MaterialName,
                    Description = src.MaterialDescription
                }));
            CreateMap<BlanketModel, ModelReadDTO>()
    .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Materials.MaterialName))
    .ForMember(dest => dest.MaterialDescription, opt => opt.MapFrom(src => src.Materials.Description));
        }
    }
}
