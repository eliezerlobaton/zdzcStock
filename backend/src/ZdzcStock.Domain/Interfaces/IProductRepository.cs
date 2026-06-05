using ZdzcStock.Domain.Entities;

namespace ZdzcStock.Domain.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllWithCategoryAsync(int? categoryId = null);
    Task<Product?> GetByIdAsync(int id);
    Task<bool> CategoryExistsAsync(int categoryId);
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task DeleteAsync(Product product);
}
