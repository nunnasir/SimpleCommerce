using SimpleCommerce.DAL.Interfaces;
using SimpleCommerce.Domain;

namespace SimpleCommerce.DAL.Repositories;

public class ProductRepository(SimpleCommerceDbContext dbContext)
    : GenericRepository<Product>(dbContext), IProductRepository
{
}

