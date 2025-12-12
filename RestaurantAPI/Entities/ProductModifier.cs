namespace RestaurantAPI.Entities
{
    public class ProductModifier : TenantEntity
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty; // VD: Chọn Size
        public bool IsRequired { get; set; } = false; // Bắt buộc chọn?
        public bool IsMultiple { get; set; } = false; // Chọn nhiều được không?
        public Product? Product { get; set; }
        public ICollection<ProductModifierItem> Items { get; set; } = new List<ProductModifierItem>();
    }
}