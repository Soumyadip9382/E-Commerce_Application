using E_CommerceApp.Infrastructure;
using E_CommerceApp.Interface;
using Microsoft.EntityFrameworkCore;
using static E_CommerceApp.Domain.DTOs.CategoryDTO;

public class CategoryService : ICategoryService
{
    private readonly AppDBContext _context;
    private readonly IProductService _productService;

    public CategoryService(AppDBContext context, IProductService productService)
    {
        _context = context;
        _productService = productService;
    }

    // ✅ Get Parent Categories (Landing Page)
    public async Task<List<CategoryItemDTO>> GetParentCategories()
    {
        return await _context.Categories
            .Where(c => c.ParentCategoryID == null)
            .Select(c => new CategoryItemDTO
            {
                CategoryID = c.CategoryID,
                CategoryName = c.CategoryName
            })
            .ToListAsync();
    }

    // ✅ Smart API (child OR products)
    public async Task<CategoryPageDTO> GetCategoryPage(int categoryId)
    {
        var category = await _context.Categories
            .Where(c => c.CategoryID == categoryId)
            .Select(c => new
            {
                c.CategoryID,
                c.CategoryName
            })
            .FirstOrDefaultAsync();

        if (category == null)
            return null;

        // 🔹 Check subcategories
        var subCategories = await _context.Categories
            .Where(c => c.ParentCategoryID == categoryId)
            .Select(c => new CategoryItemDTO
            {
                CategoryID = c.CategoryID,
                CategoryName = c.CategoryName
            })
            .ToListAsync();

        // ✅ If children exist
        if (subCategories.Any())
        {
            return new CategoryPageDTO
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName,
                HasChildren = true,
                SubCategories = subCategories
            };
        }

        // ✅ Otherwise fetch products
        var products = await _productService.GetProductsByCategory(categoryId);

        return new CategoryPageDTO
        {
            CategoryID = category.CategoryID,
            CategoryName = category.CategoryName,
            HasChildren = false,
            Products = products
        };
    }
}