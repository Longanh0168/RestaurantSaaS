using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.DTOs.Auth;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }

        // API Đăng ký nhân viên (Chỉ dành cho Admin/Owner đã đăng nhập)
        // Chúng ta sẽ bảo vệ API này sau
        [HttpPost("register")] 
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            // Tạm thời hardcode RestaurantId = 1 để test
            // Sau này sẽ lấy từ Token của người đang login
            await _authService.RegisterAsync(request, 1);
            return Ok(new { message = "Tạo nhân viên thành công" });
        }
    }
}