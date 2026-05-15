using System.ComponentModel.DataAnnotations;

namespace E_CommerceApp.Domain.Models
{
    public class ProductVariant
    {
        [Key]
        public int VariantID { get; set; }
        public int ProductID { get; set; }
        public string VariantName { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public decimal BasePrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public int StockQuantity { get; set; }
        public string SKUCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set;}
    }
}
