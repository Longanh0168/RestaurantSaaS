namespace RestaurantAPI.Entities
{
    public class Category : TenantEntity
    {
        public string Name { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}