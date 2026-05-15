using System.ComponentModel.DataAnnotations.Schema;
using static E_CommerceApp.Domain.DTOs.VariantDTO;

namespace E_CommerceApp.Domain.DTOs
{
    public class ProductDTO
    {
        [NotMapped]
        public class CreateProductDTO
        {
            public string ProductName { get; set; }
            public string Brand { get; set; }
            public string Description { get; set; }
            public string CategoryPath { get; set; }

            public List<ProductVariantDTO> Variants { get; set; }
        }

        [NotMapped]
        public class ProductListDTO
        {
            public int ProductID { get; set; }
            public int CategoryID { get; set; }
            public string ProductName { get; set; }
            public string Brand { get; set; }
            public string Description { get; set; }

            public string? DisplayImage { get; set; }
        }

        [NotMapped]
        public class TopDiscountedProductDTO
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }

            public int VariantId { get; set; }

            public decimal BasePrice { get; set; }
            public decimal DiscountedPrice { get; set; }
            public decimal DiscountPercentage { get; set; }

            public string ImageUrl { get; set; }
        }
    }
}
