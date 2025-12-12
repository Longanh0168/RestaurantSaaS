using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.DTOs.Catalog
{
    public class CreateProductDto
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Giá không được âm")]
        public decimal Price { get; set; }

        public string? Description { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class ProductDto // Trả về cho Frontend hiển thị menu
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string CategoryName { get; set; } = string.Empty; // Để hiển thị tên nhóm thay vì Id
        
        // Danh sách các tùy chọn (Size, Đường, Đá) đi kèm
        public List<ProductModifierDto> Modifiers { get; set; } = new();
    }

    public class ProductModifierDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // VD: Chọn Size
        public bool IsRequired { get; set; }
        public bool IsMultiple { get; set; }
        public List<ProductModifierItemDto> Items { get; set; } = new();
    }

    public class ProductModifierItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // VD: Size L
        public decimal PriceAdjustment { get; set; } // +5000
    }
}