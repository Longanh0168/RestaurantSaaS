using RestaurantAPI.Enums;

namespace RestaurantAPI.Entities
{
    public class User : TenantEntity
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Lưu chuỗi mã hóa
        public string FullName { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public decimal HourlyWage { get; set; } // Lương theo giờ
    }
}