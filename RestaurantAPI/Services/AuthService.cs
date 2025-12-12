using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Data;
using RestaurantAPI.DTOs.Auth;
using RestaurantAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace RestaurantAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            // 1. Tìm user theo username
            // Lưu ý: Cần ignore Query Filter vì lúc đăng nhập chưa biết quán nào
            var user = await _context.Users
                .IgnoreQueryFilters() 
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null) throw new Exception("Tài khoản hoặc mật khẩu không đúng");

            // 2. Kiểm tra mật khẩu (đã mã hóa)
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid) throw new Exception("Tài khoản hoặc mật khẩu không đúng");

            // 3. Nếu đúng -> Tạo Token
            var token = CreateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                FullName = user.FullName,
                Role = user.Role.ToString(),
                RestaurantId = user.RestaurantId
            };
        }

        public async Task RegisterAsync(RegisterRequestDto request, int restaurantId)
        {
            // Kiểm tra trùng username
            var exists = await _context.Users.AnyAsync(u => u.Username == request.Username && u.RestaurantId == restaurantId);
            if (exists) throw new Exception("Username đã tồn tại");

            var user = new User
            {
                Username = request.Username,
                FullName = request.FullName,
                Role = request.Role,
                RestaurantId = restaurantId, // Gán vào quán hiện tại
                // Mã hóa mật khẩu trước khi lưu
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // --- HÀM TẠO JWT TOKEN (QUAN TRỌNG) ---
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString()), // Lưu Role: Admin/Staff
                
                // LƯU RESTAURANT_ID VÀO TOKEN
                // Đây là bí mật để xử lý Multi-tenant
                new Claim("restaurant_id", user.RestaurantId.ToString()) 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtSettings:DurationInMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}