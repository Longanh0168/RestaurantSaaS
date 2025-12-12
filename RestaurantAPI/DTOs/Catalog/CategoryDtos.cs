using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.DTOs.Catalog
{
    // Request: Dùng khi Tạo/Sửa
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public int DisplayOrder { get; set; }
    }

    // Response: Dùng để trả về Frontend (có Id)
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
    }
}