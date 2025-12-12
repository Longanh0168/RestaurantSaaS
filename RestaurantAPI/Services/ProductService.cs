using RestaurantAPI.Entities;
using RestaurantAPI.Repositories;

namespace RestaurantAPI.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetMenuAsync();
        Task CreateProductAsync(Product product, int restaurantId);
    }

    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepo;

        public ProductService(IRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<IEnumerable<Product>> GetMenuAsync()
        {
            // Logic: Chỉ lấy món đang bán (IsActive = true)
            return await _productRepo.FindAsync(x => x.IsActive);
        }

        public async Task CreateProductAsync(Product product, int restaurantId)
        {
            // Gán RestaurantId từ token (hoặc tham số) vào entity
            product.RestaurantId = restaurantId;
            
            // Logic validate: Giá không được âm
            if (product.Price < 0) throw new Exception("Giá không hợp lệ");

            await _productRepo.AddAsync(product);
        }
    }
}