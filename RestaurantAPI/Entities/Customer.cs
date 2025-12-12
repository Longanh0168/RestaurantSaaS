namespace RestaurantAPI.Entities
{
    public class Customer : TenantEntity
    {
        public string Name { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int LoyaltyPoints { get; set; } = 0; // Điểm tích lũy
    }
}