using SimpleCommerce.Contract;
using SimpleCommerce.DAL.Interfaces;
using SimpleCommerce.Domain;

namespace SimpleCommerce.BLL.Services;

public class OrderService(IUnitOfWork unitOfWork) : IOrderService
{
    public async Task<OrderDto> CreateOrderAsync(string customerName, string mobile, string address, IReadOnlyList<CartItemDto> items)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerName = customerName,
            Mobile = mobile,
            Address = address,
            Status = "Pending",
            Total = items.Sum(i => i.Price * i.Quantity),
            CreatedAtUtc = DateTime.UtcNow
        };

        foreach (var item in items)
        {
            order.OrderItems.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Price = item.Price,
                Quantity = item.Quantity
            });
        }

        await unitOfWork.Orders.AddAsync(order);
        await unitOfWork.SaveChangesAsync();

        return new OrderDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            Mobile = order.Mobile,
            Address = order.Address,
            Status = order.Status,
            Total = order.Total,
            CreatedAtUtc = order.CreatedAtUtc,
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                ProductName = oi.ProductName,
                Price = oi.Price,
                Quantity = oi.Quantity
            }).ToList()
        };
    }

    public async Task<IReadOnlyList<OrderDto>> GetAllOrdersAsync()
    {
        var orders = await unitOfWork.Orders.GetAllAsync();
        return orders.Select(o => new OrderDto
        {
            Id = o.Id,
            CustomerName = o.CustomerName,
            Mobile = o.Mobile,
            Address = o.Address,
            Status = o.Status,
            Total = o.Total,
            CreatedAtUtc = o.CreatedAtUtc
        }).ToList();
    }

    public async Task<OrderDto?> GetOrderByIdAsync(Guid id)
    {
        var order = await unitOfWork.Orders.GetByIdWithItemsAsync(id);
        if (order is null)
            return null;

        return new OrderDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            Mobile = order.Mobile,
            Address = order.Address,
            Status = order.Status,
            Total = order.Total,
            CreatedAtUtc = order.CreatedAtUtc,
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                ProductName = oi.ProductName,
                Price = oi.Price,
                Quantity = oi.Quantity
            }).ToList()
        };
    }

    public async Task UpdateStatusAsync(Guid id, string status)
    {
        var order = await unitOfWork.Orders.GetByIdAsync(id);
        if (order is null)
            return;

        order.Status = status;
        unitOfWork.Orders.Update(order);
        await unitOfWork.SaveChangesAsync();
    }
}
