using SimpleCommerce.BLL.Interfaces;
using SimpleCommerce.Contract;
using SimpleCommerce.DAL.Interfaces;
using SimpleCommerce.Domain;

namespace SimpleCommerce.BLL.Services;

public class ProductService(IUnitOfWork unitOfWork) : IProductService
{
    public async Task<IReadOnlyList<ProductDto>> GetAllAsync()
    {
        var entities = await unitOfWork.Products.GetAllAsync();

        return entities
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl
            })
            .ToList();
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var entity = await unitOfWork.Products.GetByIdAsync(id);
        if (entity is null)
        {
            return null;
        }

        return new ProductDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            ImageUrl = entity.ImageUrl
        };
    }

    public async Task CreateAsync(ProductDto dto)
    {
        var entity = new Product
        {
            Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl
        };

        await unitOfWork.Products.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProductDto dto)
    {
        var entity = await unitOfWork.Products.GetByIdAsync(dto.Id);
        if (entity is null)
        {
            return;
        }

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.Price = dto.Price;
        entity.ImageUrl = dto.ImageUrl;

        unitOfWork.Products.Update(entity);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await unitOfWork.Products.GetByIdAsync(id);
        if (entity is null)
        {
            return;
        }

        unitOfWork.Products.Remove(entity);
        await unitOfWork.SaveChangesAsync();
    }
}
