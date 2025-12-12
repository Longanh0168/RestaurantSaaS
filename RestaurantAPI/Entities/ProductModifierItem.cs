namespace RestaurantAPI.Entities
{
    // Bảng này phụ thuộc hoàn toàn vào ProductModifier nên không nhất thiết cần TenantEntity 
    // nếu logic query luôn đi qua Modifier. Tuy nhiên, để an toàn ta cứ kế thừa BaseEntity 
    // và không cần TenantId vì cha nó đã có rồi.
    public class ProductModifierItem : BaseEntity 
    {
        public int ProductModifierId { get; set; }
        public string Name { get; set; } = string.Empty; // VD: Size L
        public decimal PriceAdjustment { get; set; } = 0; // Cộng thêm tiền (VD: +5000)

        public ProductModifier? ProductModifier { get; set; }
    }
}