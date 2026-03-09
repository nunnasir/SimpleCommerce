using System.ComponentModel.DataAnnotations;

namespace SimpleCommerce.Contract;

public class ProductDto
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Range(1, 9999999)]
    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }
}

