using Microsoft.AspNetCore.Mvc;

namespace E_CommerceApp.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // ✅ Get parent categories
        [HttpGet("parents")]
        public async Task<IActionResult> GetParents()
        {
            var data = await _categoryService.GetParentCategories();
            return Ok(data);
        }

        // ✅ Smart navigation API
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryPage(int id)
        {
            var result = await _categoryService.GetCategoryPage(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
