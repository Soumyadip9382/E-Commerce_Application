using E_CommerceApp.Domain.DTOs;

public interface IBannerService
{
    Task<int> CreateBanner(BannerDTO.CreateBannerDTO dto);
    Task<List<BannerDTO.BannerResponseDTO>> GetAllBanners();
    Task<BannerDTO.BannerResponseDTO?> GetBannerById(int id);
    Task<bool> UpdateBanner(int id, BannerDTO.UpdateBannerDTO dto);
    Task<bool> DeleteBanner(int id);
}