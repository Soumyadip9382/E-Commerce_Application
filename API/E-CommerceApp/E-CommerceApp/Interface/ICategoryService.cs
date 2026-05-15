using static E_CommerceApp.Domain.DTOs.CategoryDTO;

public interface ICategoryService
{
    Task<List<CategoryItemDTO>> GetParentCategories();
    Task<CategoryPageDTO> GetCategoryPage(int categoryId);
}