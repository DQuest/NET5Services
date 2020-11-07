using System.Collections.Generic;
using Homework2.ImageService.Entities;
using Homework2.ImageService.Models;
using Microsoft.EntityFrameworkCore;

namespace Homework2.ImageService
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