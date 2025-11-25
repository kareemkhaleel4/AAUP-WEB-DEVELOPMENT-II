using Microsoft.EntityFrameworkCore;
using EfAspDemo.Models;

namespace EfAspDemo.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}