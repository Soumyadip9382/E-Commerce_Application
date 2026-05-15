using System.ComponentModel.DataAnnotations;

namespace E_CommerceApp.Domain.Models
{
    public class ProductImage
    {
        [Key]
        public int ProductImageID { get; set; }
        public int VariantID { get; set; }
        public string ImageURL { get; set; }
        public bool IsPrimary { get; set; }
        public int ImageOrder { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
