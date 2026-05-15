using E_CommerceApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using static E_CommerceApp.Domain.DTOs.BannerDTO;

namespace E_CommerceApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private readonly BannerService _bannerService;
        public BannerController(BannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet("/banners")]
        public async Task<List<BannerResponseDTO>> GetAllBanners()
        {
            return await _bannerService.GetAllBanners();
        }

        [HttpPost("/createbanner")]
        public async Task<int> CreateBanner(CreateBannerDTO dto)
        {
            try
            {
                var bannerid = await _bannerService.CreateBanner(dto);
                return bannerid;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("/getbanner/{id}")]
        public async Task<BannerResponseDTO?> GetBannerById(int id)
        {
            try
            {
                return await _bannerService.GetBannerById(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPut("/updatebanner/{id}")]
        public async Task<bool> UpdateBanner(int id, UpdateBannerDTO dto)
        {
            try
            {
                return await _bannerService.UpdateBanner(id, dto);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpDelete("/deletebanner/{id}")]
        public async Task<bool> DeleteBanner(int id)
        {
            try
            {
                return await _bannerService.DeleteBanner(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
