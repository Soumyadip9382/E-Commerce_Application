using System.ComponentModel.DataAnnotations.Schema;
using static E_CommerceApp.Domain.DTOs.ProductDTO;

namespace E_CommerceApp.Domain.DTOs
{
    public class CategoryDTO
    {
        [NotMapped]
        public class CategoryItemDTO
        {
            public int CategoryID { get; set; }
            public string CategoryName { get; set; }
        }

        [NotMapped]
        public class CategoryPageDTO
        {
            public int CategoryID { get; set; }
            public string CategoryName { get; set; }
            public bool HasChildren { get; set; }

            public List<CategoryItemDTO>? SubCategories { get; set; }
            public List<ProductListDTO>? Products { get; set; }
        }

    }
}
