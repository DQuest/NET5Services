using ImageService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageService
{
    public class ImageContext : DbContext
    {
        public DbSet<ImageEntity> Image { get; set; }

        public ImageContext(DbContextOptions<ImageContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}