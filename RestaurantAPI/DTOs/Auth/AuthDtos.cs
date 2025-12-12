using System.ComponentModel.DataAnnotations;
using RestaurantAPI.Enums;

namespace RestaurantAPI.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty; // JWT Token
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int RestaurantId { get; set; } // Để Frontend lưu context
    }

    public class RegisterRequestDto // Dùng để tạo user mới
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.Staff;
    }
}