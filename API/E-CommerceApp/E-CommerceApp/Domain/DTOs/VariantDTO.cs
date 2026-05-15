using E_CommerceApp.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceApp.Domain.DTOs
{
    public class VariantDTO
    {
        [NotMapped]
        public class ProductVariantDTO
        {
            public string VariantName { get; set; }
            public string Size { get; set; }
            public string Color { get; set; }
            public decimal BasePrice { get; set; }
            public decimal DiscountedPrice { get; set; }
            public int StockQuantity { get; set; }

            public List<ProductImageDTO> Images { get; set; }
        }

        [NotMapped]
        public class ProductImageDTO
        {
            public string ImageURL { get; set; }
            public bool IsPrimary { get; set; }
            public int ImageOrder { get; set; }
        }

        [NotMapped]
        public class ProductVariantResponseDTO
        {
            public ProductVariant Variants { get; set; }
            public List<ProductImageDTO> Images { get; set; }
        }

        [NotMapped]
        public class UpdateVariantDTO
        {
            public decimal? BasePrice { get; set; }
            public decimal? DiscountedPrice { get; set; }
            public int? StockQuantity { get; set; }
            public bool? IsActive { get; set; }
        }
    }
}
