using E_CommerceApp.Domain.DTOs;
using E_CommerceApp.Domain.Models;
using E_CommerceApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

public class BannerService : IBannerService
{
    private readonly AppDBContext _context;

    public BannerService(AppDBContext context)
    {
        _context = context;
    }

    // ✅ CREATE
    public async Task<int> CreateBanner(BannerDTO.CreateBannerDTO dto)
    {
        ValidateRedirect(dto.RedirectType, dto.RedirectId, dto.RedirectUrl);

        var banner = new Banner
        {
            Title = dto.Title,
            ImageUrl = dto.ImageUrl,
            RedirectType = dto.RedirectType,
            RedirectId = dto.RedirectId,
            RedirectUrl = dto.RedirectUrl,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            IsActive = dto.IsActive,
            DisplayOrder = dto.DisplayOrder,
            CreatedAt = DateTime.UtcNow
        };

        _context.Banners.Add(banner);
        await _context.SaveChangesAsync();

        return banner.BannerID;
    }

    // ✅ GET ALL (only active + valid date banners)
    public async Task<List<BannerDTO.BannerResponseDTO>> GetAllBanners()
    {
        var now = DateTime.UtcNow;

        return await _context.Banners
            .Where(b =>
                b.IsActive &&
                (b.StartDate == null || b.StartDate <= now) &&
                (b.EndDate == null || b.EndDate >= now))
            .OrderBy(b => b.DisplayOrder)
            .Select(b => new BannerDTO.BannerResponseDTO
            {
                BannerID = b.BannerID,
                Title = b.Title,
                ImageUrl = b.ImageUrl,
                RedirectType = b.RedirectType,
                RedirectId = b.RedirectId,
                RedirectUrl = b.RedirectUrl,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                IsActive = b.IsActive,
                DisplayOrder = b.DisplayOrder,
                CreatedAt = b.CreatedAt
            })
            .ToListAsync();
    }

    // ✅ GET BY ID
    public async Task<BannerDTO.BannerResponseDTO?> GetBannerById(int id)
    {
        return await _context.Banners
            .Where(b => b.BannerID == id)
            .Select(b => new BannerDTO.BannerResponseDTO
            {
                BannerID = b.BannerID,
                Title = b.Title,
                ImageUrl = b.ImageUrl,
                RedirectType = b.RedirectType,
                RedirectId = b.RedirectId,
                RedirectUrl = b.RedirectUrl,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                IsActive = b.IsActive,
                DisplayOrder = b.DisplayOrder,
                CreatedAt = b.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    // ✅ UPDATE
    public async Task<bool> UpdateBanner(int id, BannerDTO.UpdateBannerDTO dto)
    {
        var banner = await _context.Banners.FindAsync(id);
        if (banner == null) return false;

        // Partial update
        if (dto.Title != null) banner.Title = dto.Title;
        if (dto.ImageUrl != null) banner.ImageUrl = dto.ImageUrl;
        if (dto.RedirectType != null) banner.RedirectType = dto.RedirectType;
        if (dto.RedirectId.HasValue) banner.RedirectId = dto.RedirectId;
        if (dto.RedirectUrl != null) banner.RedirectUrl = dto.RedirectUrl;
        if (dto.StartDate.HasValue) banner.StartDate = dto.StartDate;
        if (dto.EndDate.HasValue) banner.EndDate = dto.EndDate;
        if (dto.IsActive.HasValue) banner.IsActive = dto.IsActive.Value;
        if (dto.DisplayOrder.HasValue) banner.DisplayOrder = dto.DisplayOrder.Value;

        ValidateRedirect(banner.RedirectType, banner.RedirectId, banner.RedirectUrl);

        await _context.SaveChangesAsync();
        return true;
    }

    // ✅ DELETE
    public async Task<bool> DeleteBanner(int id)
    {
        var banner = await _context.Banners.FindAsync(id);
        if (banner == null) return false;

        _context.Banners.Remove(banner);
        await _context.SaveChangesAsync();

        return true;
    }

    // 🔥 VALIDATION LOGIC (IMPORTANT)
    private void ValidateRedirect(string type, int? id, string? url)
    {
        switch (type?.ToUpper())
        {
            case "PRODUCT":
            case "CATEGORY":
                if (!id.HasValue)
                    throw new Exception($"{type} requires RedirectId");
                break;

            case "URL":
                if (string.IsNullOrWhiteSpace(url))
                    throw new Exception("URL requires RedirectUrl");
                break;

            default:
                throw new Exception("Invalid RedirectType (PRODUCT / CATEGORY / URL)");
        }
    }
}