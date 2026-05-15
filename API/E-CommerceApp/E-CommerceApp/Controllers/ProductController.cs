using E_CommerceApp.Domain.Models;
using E_CommerceApp.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static E_CommerceApp.Domain.DTOs.ProductDTO;

namespace E_CommerceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<List<ProductListDTO>> GetProducts()
        {
            return await _productService.GetAllProducts();
        }

        [HttpPost("/CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] Domain.DTOs.ProductDTO.CreateProductDTO dto)
        {
            try
            {
                int productId = await _productService.CreateProductAsync(dto);
                return Ok(new { ProductID = productId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("/GetProductsByCategory/{categoryid}")]
        public async Task<List<ProductListDTO>> GetProductsByCategory(int categoryid)
        {
            return await _productService.GetProductsByCategory(categoryid);
        }

        [HttpGet("/GetTopDiscountedProducts")]
        public async Task<List<TopDiscountedProductDTO>> GetTopDiscountedProducts()
        {
            return await _productService.GetTopDiscountedProducts();
        }

        [HttpGet("/GetTopDiscountedProducts/{categoryid}")]
        public async Task<List<TopDiscountedProductDTO>> GetTopDiscountedProducts(int categoryid)
        {
            return await _productService.GetTopDiscountedProducts(categoryid);
        }
    }
}
