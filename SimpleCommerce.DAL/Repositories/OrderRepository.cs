using Microsoft.EntityFrameworkCore;
using SimpleCommerce.DAL.Interfaces;
using SimpleCommerce.Domain;

namespace SimpleCommerce.DAL.Repositories;

public class OrderRepository(SimpleCommerceDbContext dbContext)
    : GenericRepository<Order>(dbContext), IOrderRepository
{
    public async Task<Order?> GetByIdWithItemsAsync(Guid id)
    {
        return await DbSet
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
}
