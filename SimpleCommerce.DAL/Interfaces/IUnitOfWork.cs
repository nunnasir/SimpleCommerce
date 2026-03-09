namespace SimpleCommerce.DAL.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    Task<int> SaveChangesAsync();
}

