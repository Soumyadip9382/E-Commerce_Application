using static E_CommerceApp.Domain.DTOs.VariantDTO;

namespace E_CommerceApp.Interface
{
    public interface IProductVariantService
    {
        Task<List<ProductVariantResponseDTO>> GetProductVariants(int productId);
        Task<ProductVariantResponseDTO> GetProductVariantById(int productId, int variantId);
        Task<ProductVariantResponseDTO> UpdateVariant(int productId, int variantId, UpdateVariantDTO updateVariantDTO);
        Task<ProductVariantResponseDTO> CreateVariant(int productId, ProductVariantDTO dto);
        Task<bool> DeleteVariant(int productId, int variantId);
    }
}
