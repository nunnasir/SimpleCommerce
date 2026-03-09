namespace SimpleCommerce.Web.Models;

public class CartItem
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int Quantity { get; set; }

    public decimal LineTotal => Price * Quantity;
}
