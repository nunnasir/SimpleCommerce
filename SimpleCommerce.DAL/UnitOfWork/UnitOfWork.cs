using SimpleCommerce.DAL.Interfaces;
using SimpleCommerce.DAL.Repositories;

namespace SimpleCommerce.DAL.UnitOfWork;

public class UnitOfWork(SimpleCommerceDbContext dbContext) : IUnitOfWork
{
    private readonly SimpleCommerceDbContext _dbContext = dbContext;

    private IProductRepository? _productRepository;
    private IOrderRepository? _orderRepository;

    public IProductRepository Products => _productRepository ??= new ProductRepository(_dbContext);
    public IOrderRepository Orders => _orderRepository ??= new OrderRepository(_dbContext);

    public Task<int> SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}

