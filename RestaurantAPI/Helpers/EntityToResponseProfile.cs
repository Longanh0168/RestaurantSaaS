using AutoMapper;
using RestaurantAPI.DTOs.Catalog;
using RestaurantAPI.DTOs.Orders;
using RestaurantAPI.DTOs.Tables;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Helpers
{
    public class EntityToResponseProfile : Profile
    {
        public EntityToResponseProfile()
        {
            // --- CATALOG MODULE ---
            CreateMap<Category, CategoryDto>();
            
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "N/A"));

            CreateMap<ProductModifier, ProductModifierDto>();
            CreateMap<ProductModifierItem, ProductModifierItemDto>();

            // --- TABLE MODULE ---
            CreateMap<DiningTable, TableDto>()
                .ForMember(dest => dest.AreaName, opt => opt.MapFrom(src => src.Area != null ? src.Area.Name : "Chưa phân khu"));

            // --- ORDER MODULE ---
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.TableName, opt => opt.MapFrom(src => src.Table != null ? src.Table.Name : "Mang về"))
                .ForMember(dest => dest.StaffName, opt => opt.MapFrom(src => src.Staff != null ? src.Staff.FullName : ""));

            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.Modifiers, opt => opt.MapFrom(src => src.Modifiers.Select(m => m.ModifierName).ToList()));
        }
    }
}