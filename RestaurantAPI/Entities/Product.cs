using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantAPI.Entities
{
    public class Product : TenantEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        // Foreign Key ví dụ (Category)
        public int CategoryId { get; set; }
        // public Category Category { get; set; } // Navigation property
    }
}