namespace SimpleCommerce.DAL.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    IProductRepository Products { get; }
    Task<int> SaveChangesAsync();
}

