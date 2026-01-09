using Market.api.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories=> Set<Category>();
    }
}