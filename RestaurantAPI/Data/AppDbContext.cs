using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Enums;

namespace RestaurantAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // --- KHAI BÁO DBSET ---
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductModifier> ProductModifiers { get; set; }
        public DbSet<ProductModifierItem> ProductModifierItems { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<DiningTable> DiningTables { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderDetailModifier> OrderDetailModifiers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =================================================================
            // 1. CẤU HÌNH GLOBAL QUERY FILTER (MULTI-TENANT) - BẰNG REFLECTION
            // =================================================================
            // Thay vì viết thủ công từng dòng, ta dùng Reflection để tự động áp dụng 
            // cho tất cả Entity có kế thừa IMustHaveTenant.
            
            // TODO: Sau này biến này sẽ lấy từ Service (User đang đăng nhập)
            int currentRestaurantId = 1;

            // Lấy danh sách tất cả các Entity trong hệ thống
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Kiểm tra xem Entity đó có chứa property RestaurantId không
                if (typeof(IMustHaveTenant).IsAssignableFrom(entityType.ClrType))
                {
                    // Áp dụng filter: e => e.RestaurantId == currentRestaurantId
                    var method = typeof(AppDbContext)
                        .GetMethod(nameof(SetGlobalQueryFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                        ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(null, new object[] { modelBuilder, currentRestaurantId });
                }
            }

            // =================================================================
            // 2. CẤU HÌNH KIỂU DỮ LIỆU (DECIMAL CHO TIỀN TỆ)
            // =================================================================
            // SQL Server mặc định decimal(18,2) nhưng tốt nhất nên khai báo rõ ràng
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            // =================================================================
            // 3. CẤU HÌNH QUAN HỆ & KHÓA NGOẠI (RELATIONSHIPS)
            // =================================================================

            // --- CATEGORY & PRODUCT ---
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne() // Product.Category (nếu bạn có navigation property ngược lại thì điền vào, vd: p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Xóa Category KHÔNG được xóa Product (để an toàn dữ liệu)

            // --- PRODUCT & MODIFIER ---
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Modifiers) // Giả sử trong Product có List<ProductModifier> Modifiers
                .WithOne(m => m.Product)
                .HasForeignKey(m => m.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Món -> Xóa luôn các tùy chọn đi kèm

            modelBuilder.Entity<ProductModifier>()
                .HasMany(m => m.Items)
                .WithOne(i => i.ProductModifier)
                .HasForeignKey(i => i.ProductModifierId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- AREA & TABLE ---
            modelBuilder.Entity<Area>()
                .HasMany(a => a.DiningTables)
                .WithOne(t => t.Area)
                .HasForeignKey(t => t.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- ORDER & CÁC LIÊN KẾT ---
            modelBuilder.Entity<Order>(entity =>
            {
                // Order - Table (Table bị xóa -> Order giữ lại nhưng TableId = null)
                entity.HasOne(o => o.Table)
                      .WithMany()
                      .HasForeignKey(o => o.TableId)
                      .OnDelete(DeleteBehavior.SetNull);

                // Order - Customer (Customer bị xóa -> Order giữ lại nhưng CustomerId = null)
                entity.HasOne(o => o.Customer)
                      .WithMany()
                      .HasForeignKey(o => o.CustomerId)
                      .OnDelete(DeleteBehavior.SetNull);

                // Order - Staff (Nhân viên nghỉ việc/bị xóa -> Order vẫn còn lịch sử)
                entity.HasOne(o => o.Staff)
                      .WithMany()
                      .HasForeignKey(o => o.StaffId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // --- ORDER & ORDER DETAIL ---
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa đơn hàng -> Xóa hết chi tiết (hợp lý)

            // --- ORDER DETAIL & PRODUCT ---
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany()
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict); 
            // Quan trọng: Không cho phép xóa Product nếu đã từng có đơn hàng bán món đó.
            // Muốn xóa món, chỉ được set IsActive = false.

            modelBuilder.Entity<OrderDetail>()
                .HasMany(od => od.Modifiers)
                .WithOne(m => m.OrderDetail)
                .HasForeignKey(m => m.OrderDetailId)
                .OnDelete(DeleteBehavior.Cascade);

            // =================================================================
            // 4. CẤU HÌNH KHÁC (ENUM CONVERSION)
            // =================================================================
            // Lưu Enum dưới DB dạng chuỗi (dễ đọc) thay vì số int (khó debug) - Tùy chọn
            // Ở đây tôi giữ mặc định là int cho hiệu năng, nhưng bạn có thể đổi nếu muốn.

            // =================================================================
            // 5. CẤU HÌNH INDEX (TỐI ƯU HIỆU NĂNG)
            // =================================================================

            // A. TỰ ĐỘNG ĐÁNH INDEX CHO CỘT RestaurantId
            // -----------------------------------------------------------------
            // Vì 99% câu query đều có "WHERE RestaurantId = ...", ta cần index cột này ở mọi bảng.
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IMustHaveTenant).IsAssignableFrom(entityType.ClrType))
                {
                    // Tương đương: modelBuilder.Entity<T>().HasIndex(x => x.RestaurantId);
                    modelBuilder.Entity(entityType.ClrType)
                        .HasIndex(nameof(TenantEntity.RestaurantId)); 
                }
            }

            // B. ĐÁNH INDEX GHÉP (COMPOSITE INDEX) CHO CÁC BẢNG QUAN TRỌNG
            // -----------------------------------------------------------------
            // Nguyên tắc: Cột dùng để lọc (WHERE) nhiều nhất đặt trước, cột sắp xếp (ORDER BY) đặt sau.

            // 1. BẢNG ORDERS (Dữ liệu lớn nhất - Cần tối ưu báo cáo)
            modelBuilder.Entity<Order>(e =>
            {
                // Tối ưu cho việc lấy lịch sử đơn hàng theo thời gian của 1 quán
                // Query: WHERE RestaurantId=1 AND CreatedTime BETWEEN ...
                e.HasIndex(o => new { o.RestaurantId, o.CreatedTime });

                // Tối ưu cho việc lọc theo trạng thái (Bếp xem món chưa làm)
                // Query: WHERE RestaurantId=1 AND Status=Pending
                e.HasIndex(o => new { o.RestaurantId, o.Status });

                // Tối ưu tìm kiếm theo Mã đơn hàng
                e.HasIndex(o => new { o.RestaurantId, o.OrderCode });
            });

            // 2. BẢNG CUSTOMERS (Tra cứu khách hàng nhanh)
            modelBuilder.Entity<Customer>(e =>
            {
                // Tìm khách bằng SĐT phải nhanh tức thì (khi nhập số tại quầy)
                // Và đảm bảo 1 quán không lưu trùng SĐT khách (Unique per Restaurant)
                e.HasIndex(c => new { c.RestaurantId, c.PhoneNumber })
                .IsUnique(); 
            });

            // 3. BẢNG PRODUCTS (Hiển thị Menu)
            modelBuilder.Entity<Product>(e =>
            {
                // Tối ưu load menu theo danh mục
                e.HasIndex(p => new { p.RestaurantId, p.CategoryId });
                
                // Tối ưu tìm kiếm món ăn theo tên
                e.HasIndex(p => new { p.RestaurantId, p.Name });
            });

            // 4. BẢNG VOUCHERS
            modelBuilder.Entity<Voucher>(e =>
            {
                // Check mã giảm giá nhanh và không trùng mã trong cùng 1 quán
                e.HasIndex(v => new { v.RestaurantId, v.Code })
                .IsUnique();
            });

            // 5. BẢNG USERS (Đăng nhập)
            modelBuilder.Entity<User>(e =>
            {
                // Username là duy nhất trong toàn hệ thống (hoặc trong 1 quán tùy logic)
                // Ở đây tôi để duy nhất trong 1 quán.
                e.HasIndex(u => new { u.RestaurantId, u.Username })
                .IsUnique();
            });
            
            // 6. BẢNG DINING TABLES
            modelBuilder.Entity<DiningTable>(e => 
            {
                // Trong 1 quán, tên bàn không được trùng nhau (tránh nhầm lẫn)
                e.HasIndex(t => new { t.RestaurantId, t.Name })
                .IsUnique();
            });
        }

        // --- HELPER METHOD CHO REFLECTION ---
        // Hàm này dùng để tạo filter động cho các bảng Tenant
        static void SetGlobalQueryFilter<T>(ModelBuilder builder, int restaurantId) where T : class, IMustHaveTenant
        {
            builder.Entity<T>().HasQueryFilter(e => e.RestaurantId == restaurantId);
        }
    }
}