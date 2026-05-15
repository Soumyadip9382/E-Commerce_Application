using E_CommerceApp.Domain.DTOs;
using E_CommerceApp.Domain.Models;
using E_CommerceApp.Helper;
using E_CommerceApp.Infrastructure;
using E_CommerceApp.Interface;
using Microsoft.EntityFrameworkCore;
using static E_CommerceApp.Domain.DTOs.ProductDTO;
using static E_CommerceApp.Helper.GenerateSKUCodeHelper;

namespace E_CommerceApp.Services
{
    public class ProductService: IProductService
    {
        private readonly AppDBContext _context;
        private readonly ResolveCategoryPathHelper _categoryHelper;
        private readonly GenerateSKUCodeHelper _skuHelper;
        public ProductService(AppDBContext context, ResolveCategoryPathHelper categoryHelper, GenerateSKUCodeHelper skuHelper)
        {
            _context = context;
            _categoryHelper = categoryHelper;
            _skuHelper = skuHelper;
        }

        public async Task<List<ProductListDTO>> GetAllProducts()
        {
            // 🔹 Step 1: Get products + their variant IDs
            var productData = await _context.Products
                .Select(p => new
                {
                    p.ProductID,
                    p.CategoryID,
                    p.ProductName,
                    p.Brand,
                    p.Description,
                    VariantIds = _context.ProductVariants
                        .Where(v => v.ProductID == p.ProductID)
                        .Select(v => v.VariantID)
                        .ToList()
                })
                .ToListAsync();

            // 🔹 Step 2: Collect all variant IDs
            var allVariantIds = productData
                .SelectMany(p => p.VariantIds)
                .Distinct()
                .ToList();

            // 🔹 Step 3: Get all primary images for those variants
            var images = await _context.ProductImages
                .Where(img => allVariantIds.Contains(img.VariantID) && img.IsPrimary)
                .Select(img => new
                {
                    img.VariantID,
                    img.ImageURL
                })
                .ToListAsync();

            // 🔹 Step 4: Map + pick random image per product
            var result = productData.Select(p =>
            {
                var productImages = images
                    .Where(img => p.VariantIds.Contains(img.VariantID))
                    .Select(img => img.ImageURL)
                    .ToList();

                return new ProductListDTO
                {
                    ProductID = p.ProductID,
                    CategoryID = p.CategoryID,
                    ProductName = p.ProductName,
                    Brand = p.Brand,
                    Description = p.Description,
                    DisplayImage = productImages.Any()
                        ? productImages.OrderBy(x => Guid.NewGuid()).First()
                        : null
                };
            }).ToList();

            return result;
        }

        public async Task<int> CreateProductAsync(ProductDTO.CreateProductDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 🔹 Step 1: Resolve Category
                int categoryId = await _categoryHelper.ResolveCategoryPath(dto.CategoryPath);

                // 🔹 Step 2: Get or Create Product
                var product = await _context.Products
                    .FirstOrDefaultAsync(p =>
                        p.ProductName == dto.ProductName &&
                        p.Brand == dto.Brand);

                if (product == null)
                {
                    product = new Product
                    {
                        ProductName = dto.ProductName,
                        Brand = dto.Brand,
                        Description = dto.Description,
                        CategoryID = categoryId,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Products.Add(product);
                    await _context.SaveChangesAsync(); // needed to get ProductID
                }

                // 🔹 Step 3: Process Variants
                foreach (var v in dto.Variants)
                {
                    string sku = SkuGenerator.GenerateSKU(dto.ProductName,dto.Brand,dto.CategoryPath,v.Color,v.Size);

                    var existingVariant = await _context.ProductVariants
                        .FirstOrDefaultAsync(x => x.SKUCode == sku);

                    if (existingVariant != null)
                    {
                        // ✅ UPDATE existing variant
                        existingVariant.BasePrice = v.BasePrice;
                        existingVariant.DiscountedPrice = v.DiscountedPrice;
                        existingVariant.StockQuantity = (v.StockQuantity + existingVariant.StockQuantity);
                        existingVariant.CreatedAt = DateTime.UtcNow;
                        existingVariant.UpdatedAt = DateTime.UtcNow;
                    }
                    else
                    {
                        // ✅ INSERT new variant
                        var variant = new ProductVariant
                        {
                            ProductID = product.ProductID,
                            VariantName = v.VariantName,
                            Size = v.Size,
                            Color = v.Color,
                            BasePrice = v.BasePrice,
                            DiscountedPrice = v.DiscountedPrice,
                            StockQuantity = v.StockQuantity,
                            SKUCode = sku,
                            IsActive = true,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        _context.ProductVariants.Add(variant);
                        await _context.SaveChangesAsync();

                        foreach (var img in v.Images)
                        {
                            _context.ProductImages.Add(new ProductImage
                            {
                                VariantID = variant.VariantID,
                                ImageURL = img.ImageURL,
                                IsPrimary = img.IsPrimary,
                                ImageOrder = img.ImageOrder,
                                CreatedAt = DateTime.UtcNow
                            });
                        }
                    }
                }

                // 🔹 Final Save
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return product.ProductID;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<ProductListDTO>> GetProductsByCategory(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryID == categoryId)
                .Select(p => new ProductListDTO
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    DisplayImage = _context.ProductVariants
                        .Where(v => v.ProductID == p.ProductID)
                        .SelectMany(v => _context.ProductImages
                            .Where(img => img.VariantID == v.VariantID && img.IsPrimary))
                        .OrderBy(x => Guid.NewGuid())
                        .Select(img => img.ImageURL)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }

        public async Task<List<TopDiscountedProductDTO>> GetTopDiscountedProducts(int? categoryId = null)
        {
            List<int> categoryIds = null;

            // 👉 If category is provided, include children
            if (categoryId.HasValue)
            {
                categoryIds = await GetAllChildCategoryIds(categoryId.Value);
                //categoryIds.Add(categoryId.Value);
            }

            var query = await (
                from v in _context.ProductVariants
                join p in _context.Products on v.ProductID equals p.ProductID

                where v.BasePrice > 0
                      && v.DiscountedPrice < v.BasePrice
                      && (categoryIds == null || categoryIds.Contains(p.CategoryID))

                let discount = ((v.BasePrice - v.DiscountedPrice) / v.BasePrice) * 100

                select new
                {
                    p.ProductID,
                    p.ProductName,

                    v.VariantID,
                    v.BasePrice,
                    v.DiscountedPrice,
                    Discount = discount,

                    Image = _context.ProductImages
                        .Where(img => img.VariantID == v.VariantID && img.IsPrimary)
                        .Select(img => img.ImageURL)
                        .FirstOrDefault()
                }
            ).ToListAsync();

            // 👉 Pick best variant per product
            var result = query
                .GroupBy(x => x.ProductID)
                .Select(g => g.OrderByDescending(x => x.Discount).First())
                .OrderByDescending(x => x.Discount)
                .Take(10)
                .Select(x => new TopDiscountedProductDTO
                {
                    ProductId = x.ProductID,
                    ProductName = x.ProductName,

                    VariantId = x.VariantID,
                    BasePrice = x.BasePrice,
                    DiscountedPrice = x.DiscountedPrice,
                    DiscountPercentage = Math.Round(x.Discount, 2),

                    ImageUrl = x.Image
                })
                .ToList();

            return result;
        }

        private async Task<List<int>> GetAllChildCategoryIds(int parentCategoryId)
        {
            var result = new List<int> { parentCategoryId };

            var children = await _context.Categories
                .Where(c => c.ParentCategoryID == parentCategoryId)
                .Select(c => c.CategoryID)
                .ToListAsync();

            foreach (var child in children)
            {
                var subChildren = await GetAllChildCategoryIds(child);
                result.AddRange(subChildren);
            }

            return result;
        }
    }
}
