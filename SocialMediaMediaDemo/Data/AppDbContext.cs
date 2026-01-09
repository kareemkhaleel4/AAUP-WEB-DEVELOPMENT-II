using Microsoft.EntityFrameworkCore;
using SocialMediaMediaDemo.Models;

namespace SocialMediaMediaDemo.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<MediaBlob> MediaBlobs { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}