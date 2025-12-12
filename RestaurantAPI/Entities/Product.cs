using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class Product : TenantEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; } 
        public bool IsActive { get; set; } = true;

        // Foreign Key: Category (Một món thuộc 1 danh mục)
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Navigation Property: Modifiers (Một món có nhiều tùy chọn đi kèm)
        public ICollection<ProductModifier> Modifiers { get; set; } = new List<ProductModifier>();
    }
}