namespace RestaurantAPI.Entities
{
    public interface IMustHaveTenant
    {
        int RestaurantId { get; set; }
    }

    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
    }

    // Class này áp dụng cho các bảng cần chia dữ liệu theo Nhà hàng
    public abstract class TenantEntity : BaseEntity, IMustHaveTenant
    {
        public int RestaurantId { get; set; }
    }
}