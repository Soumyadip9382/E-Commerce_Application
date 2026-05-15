using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceApp.Domain.DTOs
{
    public class BannerDTO
    {
        [NotMapped]
        public class CreateBannerDTO
        {
            public string Title { get; set; }
            public string ImageUrl { get; set; }

            // PRODUCT / CATEGORY / URL
            public string RedirectType { get; set; }

            public int? RedirectId { get; set; }   // ProductId / CategoryId
            public string? RedirectUrl { get; set; } // External link

            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            public bool IsActive { get; set; } = true;
            public int DisplayOrder { get; set; }
        }
        [NotMapped]
        public class UpdateBannerDTO
        {
            public string? Title { get; set; }
            public string? ImageUrl { get; set; }

            public string? RedirectType { get; set; }
            public int? RedirectId { get; set; }
            public string? RedirectUrl { get; set; }

            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            public bool? IsActive { get; set; }
            public int? DisplayOrder { get; set; }
        }

        [NotMapped]
        public class BannerResponseDTO
        {
            public int BannerID { get; set; }
            public string Title { get; set; }
            public string ImageUrl { get; set; }

            public string RedirectType { get; set; }
            public int? RedirectId { get; set; }
            public string? RedirectUrl { get; set; }

            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }

            public bool IsActive { get; set; }
            public int DisplayOrder { get; set; }

            public DateTime CreatedAt { get; set; }
        }
    }
}
