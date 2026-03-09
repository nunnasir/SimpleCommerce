namespace SimpleCommerce.Contract;

public class OrderDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new();
}
