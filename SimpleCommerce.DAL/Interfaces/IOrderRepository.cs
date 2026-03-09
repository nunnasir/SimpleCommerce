using SimpleCommerce.Domain;

namespace SimpleCommerce.DAL.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<Order?> GetByIdWithItemsAsync(Guid id);
}
