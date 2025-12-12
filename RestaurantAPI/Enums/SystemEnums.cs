namespace RestaurantAPI.Enums
{
    public enum OrderStatus
    {
        Pending = 0,    // Chờ xác nhận
        Confirmed = 1,  // Bếp đã nhận
        Processing = 2, // Đang chế biến
        Served = 3,     // Đã ra món
        Completed = 4,  // Đã thanh toán xong
        Cancelled = -1  // Hủy
    }

    public enum PaymentMethod
    {
        Cash = 0,
        BankTransfer = 1,
        Momo = 2,
        VNPay = 3
    }

    public enum VoucherType
    {
        Percentage = 0, // Giảm theo %
        FixedAmount = 1 // Giảm tiền mặt (VD: 20k)
    }

    public enum UserRole
    {
        Owner = 0,
        Manager = 1,
        Staff = 2,
        Kitchen = 3
    }
}