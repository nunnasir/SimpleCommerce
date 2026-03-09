namespace SimpleCommerce.Domain;

public class Order
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Mobile { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, InProgress, Delivered
    public decimal Total { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
