namespace E_CommerceApp.Domain.Models
{
    public class Banner
    {
        public int BannerID { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string RedirectType { get; set; }
        public int? RedirectId { get; set; }
        public string? RedirectUrl { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; } 
    }
}
