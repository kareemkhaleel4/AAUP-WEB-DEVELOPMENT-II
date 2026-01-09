using System.ComponentModel.DataAnnotations;
namespace Market.api.Dtos
{
    public record CategoryDto(int Id, string Name);

    public class CreateCategoryDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty!;
    }

    public class UpdateCategoryDto
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty!;
    }
    
}