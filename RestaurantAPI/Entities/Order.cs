using RestaurantAPI.Enums;

namespace RestaurantAPI.Entities
{
    public class Order : TenantEntity
    {
        public string OrderCode { get; set; } = string.Empty; // #001
        
        public int? TableId { get; set; } // Null nếu mang về
        public int? CustomerId { get; set; } // Null nếu khách vãng lai
        public int? StaffId { get; set; } // Null nếu khách tự đặt
        
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
        public bool IsPaid { get; set; } = false;

        // Navigation
        public DiningTable? Table { get; set; }
        public Customer? Customer { get; set; }
        public User? Staff { get; set; } // Nhân viên chốt đơn
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}