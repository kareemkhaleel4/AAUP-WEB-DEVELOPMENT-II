using Market.api.Data;
using Market.api.Models;
using Market.api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Market.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _db;

    public CategoriesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<List<CategoryDto>> GetAll()
    {
        return await _db.Categories
            .Select(c => new CategoryDto(c.Id, c.Name))
            .ToListAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name
        };

        _db.Categories.Add(category);
        await _db.SaveChangesAsync();

        return Ok(new CategoryDto(category.Id, category.Name));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCategoryDto dto)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category == null)
            return NotFound();

        category.Name = dto.Name;
        await _db.SaveChangesAsync();

        return Ok(new CategoryDto(category.Id, category.Name));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) 
    {
        var category = await _db.Categories.FindAsync(id);
        if (category == null)
            return NotFound();

        _db.Categories.Remove(category);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id}/products")]
    public async Task<IActionResult> GetProductsByCategory(int id)
    {
        var categoryExists = await _db.Categories.AnyAsync(c => c.Id == id);
        if (!categoryExists)
            return NotFound();

        var products = await _db.Products
            .Where(p => p.CategoryId == id)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Price,
                p.StockQty,
                p.Category!.Name
            ))
            .ToListAsync();

        return Ok(products);
    }   
}
