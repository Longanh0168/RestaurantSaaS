using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // CẤU HÌNH GLOBAL QUERY FILTER (Bí mật của Multi-tenant)
            // Giả sử: Chúng ta sẽ lấy RestaurantId từ một Service (sẽ làm ở bước nâng cao).
            // Tạm thời tôi hardcode là 1 để test. Khi làm Auth, ta sẽ thay số 1 bằng biến động.
            int currentRestaurantId = 1; 

            // Tự động lọc tất cả các bảng có implement IMustHaveTenant
            modelBuilder.Entity<Product>().HasQueryFilter(p => p.RestaurantId == currentRestaurantId);
            
            // Lưu ý: Với hệ thống thật, bạn cần dùng Reflection để loop qua tất cả các entity
            // thay vì viết từng dòng cho từng bảng.
        }
    }
}