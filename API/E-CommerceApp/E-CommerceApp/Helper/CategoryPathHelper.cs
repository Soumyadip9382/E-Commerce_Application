using E_CommerceApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceApp.Helper
{
    public class CategoryPathHelper
    {
        private readonly AppDBContext _context;
        public CategoryPathHelper(AppDBContext context)
        {
            _context = context;
        }
        public async Task<string> GetCategoryPath(int categoryId)
        {
            var path = new List<string>();

            while (categoryId != 0)
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryID == categoryId);

                if (category == null) break;

                path.Insert(0, category.CategoryName);
                categoryId = category.ParentCategoryID ?? 0;
            }

            return string.Join(" > ", path);
        }
    }
}
