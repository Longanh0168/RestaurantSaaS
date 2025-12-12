using RestaurantAPI.DTOs.Auth;

namespace RestaurantAPI.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task RegisterAsync(RegisterRequestDto request, int restaurantId); // Chỉ Admin mới tạo được User
    }
}