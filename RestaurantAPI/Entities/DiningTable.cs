namespace RestaurantAPI.Entities
{
    public class DiningTable : TenantEntity
    {
        public string Name { get; set; } = string.Empty; // Bàn 1, Bàn 2
        public int AreaId { get; set; }
        public Guid SecretKey { get; set; } = Guid.NewGuid(); // Dùng cho QR Code
        public bool IsOccupied { get; set; } = false; // Trạng thái có khách hay không
        public Area? Area { get; set; }
    }
}