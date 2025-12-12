using AutoMapper;
using RestaurantAPI.DTOs.Catalog;
using RestaurantAPI.DTOs.Orders;
using RestaurantAPI.DTOs.Tables;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Helpers
{
    public class RequestToEntityProfile : Profile
    {
        public RequestToEntityProfile()
        {
            // --- CATALOG MODULE ---
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<CreateProductDto, Product>();

            // --- TABLE MODULE ---
            CreateMap<CreateTableDto, DiningTable>();
            
            // --- ORDER MODULE ---
            // Lưu ý: Order thường có logic phức tạp khi tạo (tính tiền, check kho) 
            // nên ít khi map thẳng từ DTO -> Entity 1-1 mà thường xử lý tay trong Service.
            // Tuy nhiên nếu map cơ bản thì vẫn để ở đây.
            CreateMap<CreateOrderDto, Order>();
            
            // Map item trong giỏ hàng sang OrderDetail
            // Logic này thường cần xử lý thêm trong Service để lấy giá từ DB (không tin client)
            // Nhưng ta cứ khai báo khung ở đây.
            CreateMap<CartItemDto, OrderDetail>(); 
        }
    }
}