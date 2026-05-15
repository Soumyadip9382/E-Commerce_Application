using System.ComponentModel;

namespace E_CommerceApp.Domain.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public string ProductName { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
