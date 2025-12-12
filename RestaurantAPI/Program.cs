using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Data;
using RestaurantAPI.Repositories;
using RestaurantAPI.Services;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// 1. Kết nối SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Đăng ký Repository & Service
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); // Generic
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            ValidateLifetime = true, // Kiểm tra hết hạn token
            ClockSkew = TimeSpan.Zero // Không cho phép lệch giờ (hết hạn là chặn ngay)
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor(); // Cần thiết để truy cập HttpContext
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Cấu hình Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // Mỗi ngày 1 file log
    .CreateLogger();

builder.Host.UseSerilog(); // Thay thế logger mặc định của .NET

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// QUAN TRỌNG: Phải đặt đúng thứ tự
app.UseAuthentication(); // 1. Xác thực (Bạn là ai?)
app.UseAuthorization();  // 2. Phân quyền (Bạn được làm gì?)
app.MapControllers();

app.UseMiddleware<RestaurantAPI.Middlewares.ExceptionMiddleware>();

app.Run();