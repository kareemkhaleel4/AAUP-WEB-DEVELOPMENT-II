using System.ComponentModel.DataAnnotations;
namespace Market.api.Dtos
{
    public record ProductDto(
        int Id,
        string Name,
        decimal Price,
        int StockQty,
        string CategoryName
    );

    public class CreateProductDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty!;

        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Range(0, 100000)]
        public int StockQty { get; set; }
    }
    
    public class UpdateProductDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty!;

        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Range(0, 100000)]
        public int StockQty { get; set; }
    }
}