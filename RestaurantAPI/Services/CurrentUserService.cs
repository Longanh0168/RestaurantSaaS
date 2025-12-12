using System.Security.Claims;

namespace RestaurantAPI.Services
{
    public interface ICurrentUserService
    {
        int? RestaurantId { get; }
        int? UserId { get; }
    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? RestaurantId
        {
            get
            {
                // Đọc claim "restaurant_id" từ Token gửi lên
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("restaurant_id");
                return claim != null ? int.Parse(claim.Value) : null;
            }
        }
        
        public int? UserId
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
                return claim != null ? int.Parse(claim.Value) : null;
            }
        }
    }
}