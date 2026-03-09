using Microsoft.EntityFrameworkCore;
using SimpleCommerce.Domain;

namespace SimpleCommerce.DAL;

public class SimpleCommerceDbContext : DbContext
{
    public SimpleCommerceDbContext(DbContextOptions<SimpleCommerceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
}
