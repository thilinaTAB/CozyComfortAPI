using AutoMapper;
using CozyComfortAPI.DTO;
using CozyComfortAPI.Model;
using CozyComfortAPI.Models; // Make sure to include this namespace for Seller and SellerOrder

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

            // --- New Mappings for Seller Orders ---

            // Simple mapping from DTO to model
            CreateMap<SellerOrderWriteDTO, SellerOrder>();

            // Mapping from model to DTO, including calculations
            CreateMap<SellerOrder, SellerOrderReadDTO>()
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.DistributorStock.BlanketModel.ModelName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.DistributorStock.BlanketModel.Price))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.DistributorStock.BlanketModel.Price * src.Quantity));
        }
    }
}