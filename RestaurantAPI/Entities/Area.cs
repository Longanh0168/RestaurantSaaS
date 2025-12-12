namespace RestaurantAPI.Entities
{
    public class Area : TenantEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<DiningTable> DiningTables { get; set; } = new List<DiningTable>();
    }
}