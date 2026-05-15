using E_CommerceApp.Domain;
using E_CommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using static E_CommerceApp.Domain.DTOs.UserDTO;

namespace E_CommerceApp.Infrastructure
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options):base (options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Banner> Banners { get; set; }

    }
}
