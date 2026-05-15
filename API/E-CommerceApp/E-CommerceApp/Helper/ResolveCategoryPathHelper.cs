using E_CommerceApp.Domain.Models;
using E_CommerceApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceApp.Helper
{
    public class ResolveCategoryPathHelper
    {
        private readonly AppDBContext _context;

        public ResolveCategoryPathHelper(AppDBContext context)
        {
            _context = context;
        }

        public async Task<int> ResolveCategoryPath(string categoryPath)
        {
            var names = categoryPath
                .Split('>')
                .Select(x => x.Trim().ToLower())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            int? parentId = null;
            Category current = null;

            foreach (var name in names)
            {
                // 🔹 Always use AsNoTracking for lookup
                current = await _context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c =>
                        c.CategoryName == name &&
                        c.ParentCategoryID == parentId);

                if (current == null)
                {
                    var newCategory = new Category
                    {
                        CategoryName = name,
                        ParentCategoryID = parentId,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Categories.Add(newCategory);

                    try
                    {
                        await _context.SaveChangesAsync();
                        current = newCategory; // now has DB-generated ID
                    }
                    catch (DbUpdateException)
                    {
                        // 🔥 Detach failed entity to avoid tracking conflict
                        _context.Entry(newCategory).State = EntityState.Detached;

                        // 🔁 Re-fetch existing category
                        current = await _context.Categories
                            .AsNoTracking()
                            .FirstAsync(c =>
                                c.CategoryName == name &&
                                c.ParentCategoryID == parentId);
                    }
                }

                parentId = current.CategoryID;
            }

            return current.CategoryID;
        }
    }
}