using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Entities;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMenu()
        {
            var menu = await _productService.GetMenuAsync();
            return Ok(menu);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            // Giả lập RestaurantId = 1 (Sau này lấy từ User Token)
            await _productService.CreateProductAsync(product, 1);
            return Ok(new { message = "Tạo món thành công" });
        }
    }
}