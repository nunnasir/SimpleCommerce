namespace SimpleCommerce.Contract;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(string customerName, string mobile, string address, IReadOnlyList<CartItemDto> items);
    Task<IReadOnlyList<OrderDto>> GetAllOrdersAsync();
    Task<OrderDto?> GetOrderByIdAsync(Guid id);
    Task UpdateStatusAsync(Guid id, string status);
}

public class CartItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
