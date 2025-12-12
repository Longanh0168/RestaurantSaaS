namespace RestaurantAPI.Entities
{
    public class OrderDetailModifier : BaseEntity
    {
        public int OrderDetailId { get; set; }
        public string ModifierName { get; set; } = string.Empty; // VD: Size L
        public decimal Price { get; set; } // Giá cộng thêm tại thời điểm đặt

        public OrderDetail? OrderDetail { get; set; }
    }
}