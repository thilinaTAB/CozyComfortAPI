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

            // Mapping from SellerOrderWriteDTO to SellerOrder
            // We ignore properties that are manually set in the SellerOrderController
            CreateMap<SellerOrderWriteDTO, SellerOrder>()
                .ForMember(dest => dest.SellerID, opt => opt.Ignore())
                .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
                .ForMember(dest => dest.Total, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            // Mapping from SellerOrder to SellerOrderReadDTO
            // This ensures ModelName and Price are correctly populated from nested objects
            CreateMap<SellerOrder, SellerOrderReadDTO>()
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.DistributorStock.BlanketModel.ModelName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.DistributorStock.BlanketModel.Price));
        }
    }
}