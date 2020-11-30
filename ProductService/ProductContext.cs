using Microsoft.EntityFrameworkCore;
using ProductService.Entities;

namespace ProductService
{
    public class ProductContext : DbContext
    {
        public DbSet<ProductEntity> Product { get; set; }

        public ProductContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}