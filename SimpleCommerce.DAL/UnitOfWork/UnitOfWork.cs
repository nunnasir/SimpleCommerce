using SimpleCommerce.DAL.Interfaces;
using SimpleCommerce.DAL.Repositories;

namespace SimpleCommerce.DAL.UnitOfWork;

public class UnitOfWork(SimpleCommerceDbContext dbContext) : IUnitOfWork
{
    private readonly SimpleCommerceDbContext _dbContext = dbContext;

    private IProductRepository? _productRepository;

    public IProductRepository Products => _productRepository ??= new ProductRepository(_dbContext);

    public Task<int> SaveChangesAsync()
    {
        return _dbContext.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}

