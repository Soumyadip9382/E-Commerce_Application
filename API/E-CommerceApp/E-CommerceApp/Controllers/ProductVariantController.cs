using E_CommerceApp.Interface;
using Microsoft.AspNetCore.Mvc;
using static E_CommerceApp.Domain.DTOs.VariantDTO;

namespace E_CommerceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantController : ControllerBase
    {
        private readonly IProductVariantService _productVariantService;
        public ProductVariantController(IProductVariantService productVariantService)
        {
            _productVariantService = productVariantService;
        }
        [HttpGet("/GetProductVariants/{productId}")]
        public async Task<List<ProductVariantResponseDTO>> GetProductVariants(int productId)
        {
            return await _productVariantService.GetProductVariants(productId);
        }

        [HttpGet("/GetProductVariantDetails/{productId}/{VariantId}")]
        public async Task<IActionResult> GetProductVariantDetails(int productId, int VariantId)
        {
            return Ok(await _productVariantService.GetProductVariantById(productId, VariantId));
        }

        [HttpPut("/UpdateVariant/{productId}/{variantId}")]
        public async Task<IActionResult> UpdateVariant(int productId, int variantId, [FromBody] UpdateVariantDTO updateVariantDTO)
        {
            try
            {
                var updatedVariant = await _productVariantService.UpdateVariant(productId, variantId, updateVariantDTO);
                return Ok(updatedVariant);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("/CreateVariant/{productId}")]
        public async Task<IActionResult> CreateVariant(int productId, [FromBody] ProductVariantDTO dto)
        {
            try
            {
                var createdVariant = await _productVariantService.CreateVariant(productId, dto);
                return Ok(createdVariant);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("/DeleteVariant/{productId}/{variantId}")]
        public async Task<IActionResult> DeleteVariant(int productId, int variantId)
        {
            try
            {
                var result = await _productVariantService.DeleteVariant(productId, variantId);
                if (result)
                    return Ok(new { Message = "Variant deleted successfully" });
                else
                    return NotFound(new { Message = "Variant not found" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
