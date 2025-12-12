using System.ComponentModel.DataAnnotations;
using RestaurantAPI.Enums;

namespace RestaurantAPI.DTOs.Orders
{
    // ================== INPUT (REQUEST) ==================

    public class CreateOrderDto
    {
        public int? TableId { get; set; } // Có thể null nếu mang về
        
        [Required]
        [MinLength(1, ErrorMessage = "Đơn hàng phải có ít nhất 1 món")]
        public List<CartItemDto> Items { get; set; } = new();

        public string? Note { get; set; } // Ghi chú chung cho cả đơn
    }

    public class CartItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, 100, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int Quantity { get; set; }

        public string? Note { get; set; } // Ghi chú từng món (VD: Không hành)

        // Danh sách ID của các topping/option đã chọn
        // VD: Khách chọn Size L (Id=10) và Thêm Trân Châu (Id=15) -> Gửi lên [10, 15]
        public List<int> SelectedModifierItemIds { get; set; } = new();
    }

    // ================== OUTPUT (RESPONSE) ==================

    public class OrderDto
    {
        public int Id { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public string TableName { get; set; } = "Mang về"; // Flatten dữ liệu
        public string StaffName { get; set; } = string.Empty;
        public DateTime CreatedTime { get; set; }
        
        public OrderStatus Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public bool IsPaid { get; set; }
        
        public decimal TotalAmount { get; set; }
        
        public List<OrderDetailDto> Details { get; set; } = new();
    }

    public class OrderDetailDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty; // Tên món lúc đặt
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; } // Quantity * UnitPrice + Topping
        public string Note { get; set; } = string.Empty;
        
        // List tên topping để in bill (VD: ["Size L", "Trân châu trắng"])
        public List<string> Modifiers { get; set; } = new();
    }
}