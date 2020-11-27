using Microsoft.EntityFrameworkCore;
using ProductService.Entities;

namespace ProductService
{
    public class ProductContext : DbContext
    {
        public DbSet<ProductEntity> Product { get; set; }

        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}