using RestaurantAPI.Enums;

namespace RestaurantAPI.Entities
{
    public class Voucher : TenantEntity
    {
        public string Code { get; set; } = string.Empty; // VD: SALE50
        public VoucherType Type { get; set; }
        public decimal Value { get; set; } // 10 (nếu là %) hoặc 50000 (nếu là tiền)
        public decimal MinOrderAmount { get; set; } // Đơn tối thiểu
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quantity { get; set; } // Số lượng mã còn lại
        public bool IsActive { get; set; } = true;
    }
}