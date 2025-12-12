namespace RestaurantAPI.Entities
{
    public class OrderDetail : TenantEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        
        // SNAPSHOT DATA (Lưu cứng dữ liệu lúc đặt)
        public string ProductName { get; set; } = string.Empty; 
        public decimal UnitPrice { get; set; } // Giá tại thời điểm đặt
        
        public int Quantity { get; set; }
        public string? Note { get; set; } // Ít đá, nhiều đường...
        
        // Trạng thái món ăn riêng lẻ (cho KDS - Màn hình bếp)
        public bool IsCooked { get; set; } = false; 

        // Navigation
        public Order? Order { get; set; }
        public Product? Product { get; set; }
        public ICollection<OrderDetailModifier> Modifiers { get; set; } = new List<OrderDetailModifier>();
    }
}