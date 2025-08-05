using AutoMapper;
using CozyComfortAPI.DTO;
using CozyComfortAPI.Model;

namespace CozyComfortAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ModelWriteDTO, BlanketModel>();
            CreateMap<BlanketModel, ModelReadDTO>()
                .ForMember(dest => dest.MaterialName, opt => opt.MapFrom(src => src.Material.MaterialName))
                .ForMember(dest => dest.MaterialDescription, opt => opt.MapFrom(src => src.Material.Description));

            CreateMap<Material, MaterialReadDTO>();
            CreateMap<MaterialWriteDTO, Material>();

            CreateMap<Distributor, DistributorReadDTO>();
            CreateMap<DistributorWriteDTO, Distributor>();
            CreateMap<DistributorStock, DistributorStockReadDTO>();
            CreateMap<DistributorStockWriteDTO, DistributorStock>();

            CreateMap<Order, OrderReadDTO>();
            CreateMap<OrderWriteDTO, Order>();
        }
    }
}
