using Homework2.ProductService.Repository;
using Microsoft.EntityFrameworkCore;

namespace Homework2.ProductService
{
    public class ProductContext : DbContext
    {
        public DbSet<ProductDbModel> Product { get; set; }

        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}