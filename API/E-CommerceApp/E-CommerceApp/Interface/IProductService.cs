using E_CommerceApp.Domain.Models;
using static E_CommerceApp.Domain.DTOs.ProductDTO;
namespace E_CommerceApp.Interface
{
    public interface IProductService
    {
        Task<List<ProductListDTO>> GetAllProducts();
        Task<int> CreateProductAsync(CreateProductDTO dto);
        Task<List<ProductListDTO>> GetProductsByCategory(int categoryid);
        Task<List<TopDiscountedProductDTO>> GetTopDiscountedProducts(int? categoryId = null);
    }
}
