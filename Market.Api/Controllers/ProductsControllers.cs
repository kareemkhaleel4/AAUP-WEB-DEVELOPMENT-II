using Market.api.Data;
using Market.api.Models;
using Market.api.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Market.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProductsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<List<ProductDto>> GetAll()
    {
        return await _db.Products
            .Include(p => p.Category)
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Price,
                p.StockQty,
                p.Category.Name
            ))
            .ToListAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        var categoryExists = await _db.Categories.AnyAsync(c => c.Id == dto.CategoryId);
        if (!categoryExists)
            return BadRequest("Invalid category");

        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            StockQty = dto.StockQty,
            CategoryId = dto.CategoryId
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return Ok();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto)   
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        var categoryExists = await _db.Categories.AnyAsync(c => c.Id == dto.CategoryId);
        if (!categoryExists)
            return BadRequest("Invalid category");

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.StockQty = dto.StockQty;
        product.CategoryId = dto.CategoryId;

        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();

        return NoContent();
    }


}
