using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.DTOs.Tables
{
    public class CreateTableDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public int AreaId { get; set; }
    }

    public class TableDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;
        public bool IsOccupied { get; set; }
        
        // URL QR Code để Frontend generate hình
        // VD: https://my-app.com/menu?tableId=5&key=...
        public string QrCodeUrl { get; set; } = string.Empty; 
    }
}