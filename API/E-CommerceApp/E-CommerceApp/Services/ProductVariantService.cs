using E_CommerceApp.Domain.DTOs;
using E_CommerceApp.Domain.Models;
using E_CommerceApp.Helper;
using E_CommerceApp.Infrastructure;
using E_CommerceApp.Interface;
using Microsoft.EntityFrameworkCore;
using static E_CommerceApp.Domain.DTOs.VariantDTO;
using static E_CommerceApp.Helper.GenerateSKUCodeHelper;

namespace E_CommerceApp.Services
{
    public class ProductVariantService: IProductVariantService
    {
        private readonly AppDBContext _context;
        private readonly CategoryPathHelper _categoryPathHelper;
        public ProductVariantService(AppDBContext context, CategoryPathHelper categoryPathHelper)
        {
            _context = context;
            _categoryPathHelper = categoryPathHelper;
        }
        public async Task<List<ProductVariantResponseDTO>> GetProductVariants(int productId)
        {
            var variants = await _context.ProductVariants
                .Where(v => v.ProductID == productId)
                .ToListAsync();

            var response = new List<ProductVariantResponseDTO>();

            foreach (var variant in variants)
            {
                var images = await _context.ProductImages
                    .Where(i => i.VariantID == variant.VariantID)
                    .Select(i => new ProductImageDTO
                    {
                        ImageURL = i.ImageURL,
                        IsPrimary = i.IsPrimary,
                        ImageOrder = i.ImageOrder
                    })
                    .ToListAsync();

                response.Add(new ProductVariantResponseDTO
                {
                    Variants = variant,
                    Images = images
                });
            }

            return response;
        }


        public async Task<ProductVariantResponseDTO> CreateVariant(int productId,ProductVariantDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 🔹 Step 1: Get Product
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductID == productId);

                if (product == null)
                    throw new Exception("Product not found");

                // 🔹 Step 2: Get Category Path
                string categoryPath = await _categoryPathHelper.GetCategoryPath(product.CategoryID);

                // 🔹 Step 3: Generate SKU
                string sku = SkuGenerator.GenerateSKU(
                    product.ProductName,
                    product.Brand,
                    categoryPath,
                    dto.Color,
                    dto.Size
                );

                // 🔹 Step 4: Check Duplicate Variant
                var existingVariant = await _context.ProductVariants
                    .FirstOrDefaultAsync(v => v.ProductID == productId && v.SKUCode == sku);

                if (existingVariant != null)
                {
                    // ✅ Update instead of duplicate
                    existingVariant.BasePrice = dto.BasePrice;
                    existingVariant.DiscountedPrice = dto.DiscountedPrice;
                    existingVariant.StockQuantity += dto.StockQuantity;

                    await _context.SaveChangesAsync();

                    var existingImages = await _context.ProductImages
                        .Where(i => i.VariantID == existingVariant.VariantID)
                        .Select(i => new VariantDTO.ProductImageDTO
                        {
                            ImageURL = i.ImageURL,
                            IsPrimary = i.IsPrimary,
                            ImageOrder = i.ImageOrder
                        })
                        .ToListAsync();

                    await transaction.CommitAsync();

                    return new VariantDTO.ProductVariantResponseDTO
                    {
                        Variants = existingVariant,
                        Images = existingImages
                    };
                }

                // 🔹 Step 5: Create Variant
                var variant = new ProductVariant
                {
                    ProductID = productId,
                    VariantName = dto.VariantName,
                    Size = dto.Size,
                    Color = dto.Color,
                    BasePrice = dto.BasePrice,
                    DiscountedPrice = dto.DiscountedPrice,
                    StockQuantity = dto.StockQuantity,
                    SKUCode = sku,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ProductVariants.Add(variant);
                await _context.SaveChangesAsync();

                // 🔹 Step 6: Add Images
                var imageList = new List<VariantDTO.ProductImageDTO>();

                if (dto.Images != null && dto.Images.Any())
                {
                    foreach (var img in dto.Images)
                    {
                        var image = new ProductImage
                        {
                            VariantID = variant.VariantID,
                            ImageURL = img.ImageURL,
                            IsPrimary = img.IsPrimary,
                            ImageOrder = img.ImageOrder,
                            CreatedAt = DateTime.UtcNow
                        };

                        _context.ProductImages.Add(image);

                        imageList.Add(new VariantDTO.ProductImageDTO
                        {
                            ImageURL = img.ImageURL,
                            IsPrimary = img.IsPrimary,
                            ImageOrder = img.ImageOrder
                        });
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new VariantDTO.ProductVariantResponseDTO
                {
                    Variants = variant,
                    Images = imageList
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ProductVariantResponseDTO> GetProductVariantById(int productId, int variantId)
        {
            var variant = await _context.ProductVariants
                .FirstOrDefaultAsync(v => v.ProductID == productId && v.VariantID == variantId);

            if (variant == null)
                return null;

            var images = await _context.ProductImages
                .Where(i => i.VariantID == variantId)
                .Select(i => new ProductImageDTO
                {
                    ImageURL = i.ImageURL,
                    IsPrimary = i.IsPrimary,
                    ImageOrder = i.ImageOrder
                })
                .ToListAsync();

            var response = new ProductVariantResponseDTO
            {
                Variants = variant,
                Images = images
            };

            return response;
        }


        public async Task<ProductVariantResponseDTO> UpdateVariant(int productId, int variantId, UpdateVariantDTO updateVariantDTO)
        {
            var variant = await _context.ProductVariants
                .FirstOrDefaultAsync(v => v.ProductID == productId && v.VariantID == variantId);

            if (variant == null)
                return null;

            if (updateVariantDTO.BasePrice.HasValue && updateVariantDTO.BasePrice.Value != 0)
                variant.BasePrice = updateVariantDTO.BasePrice.Value;

            if (updateVariantDTO.DiscountedPrice.HasValue && updateVariantDTO.DiscountedPrice.Value != 0)
                variant.DiscountedPrice = updateVariantDTO.DiscountedPrice.Value;

            if (updateVariantDTO.StockQuantity.HasValue && updateVariantDTO.StockQuantity.Value != 0)
                variant.StockQuantity = updateVariantDTO.StockQuantity.Value;

            if (updateVariantDTO.IsActive.HasValue)
                variant.IsActive = updateVariantDTO.IsActive.Value;


            await _context.SaveChangesAsync();

            var images = await _context.ProductImages
               .Where(i => i.VariantID == variantId)
               .Select(i => new ProductImageDTO
               {
                   ImageURL = i.ImageURL,
                   IsPrimary = i.IsPrimary,
                   ImageOrder = i.ImageOrder
               })
               .ToListAsync();

            var response = new ProductVariantResponseDTO
            {
                Variants = variant,
                Images = images
            };

            return response;
        }

        public async Task<bool> DeleteVariant(int productId, int variantId)
        {
            var variant = await _context.ProductVariants
                .FirstOrDefaultAsync(v => v.ProductID == productId && v.VariantID == variantId);

            if (variant == null)
                return false;

            _context.ProductVariants.Remove(variant);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
