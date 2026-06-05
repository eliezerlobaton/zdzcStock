using ZdzcStock.Application.DTOs;
using ZdzcStock.Domain.Common;
using ZdzcStock.Domain.Entities;
using ZdzcStock.Domain.Interfaces;

namespace ZdzcStock.Application.Services;

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync(int? categoryId = null)
    {
        var products = await _productRepository.GetAllWithCategoryAsync(categoryId);

        return products.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            p.CategoryId,
            p.Category != null
                ? new CategoryDto(
                    p.Category.Id,
                    p.Category.Name,
                    p.Category.Description,
                    p.Category.CreatedAt,
                    p.Category.UpdatedAt
                )
                : null,
            p.CreatedAt,
            p.UpdatedAt
        ));
    }

    public async Task<Result<ProductDto>> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return Result<ProductDto>.Failure("Produto não encontrado.", 404);

        return Result<ProductDto>.Success(
            new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.CategoryId,
                product.Category != null
                    ? new CategoryDto(
                        product.Category.Id,
                        product.Category.Name,
                        product.Category.Description,
                        product.Category.CreatedAt,
                        product.Category.UpdatedAt
                    )
                    : null,
                product.CreatedAt,
                product.UpdatedAt
            )
        );
    }

    public async Task<Result<ProductDto>> CreateAsync(CreateProductDto dto)
    {
        var categoryExists = await _productRepository.CategoryExistsAsync(dto.CategoryId);
        if (!categoryExists)
            return Result<ProductDto>.Failure("Categoria informada não existe.", 400);

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        var created = await _productRepository.CreateAsync(product);
        var createdWithNav = await _productRepository.GetByIdAsync(created.Id);
        return Result<ProductDto>.Success(MapToDto(createdWithNav!));
    }

    public async Task<Result<ProductDto>> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return Result<ProductDto>.Failure("Produto não encontrado.", 404);

        if (product.CategoryId != dto.CategoryId)
        {
            var categoryExists = await _productRepository.CategoryExistsAsync(dto.CategoryId);
            if (!categoryExists)
                return Result<ProductDto>.Failure("Categoria informada não existe..", 400);
        }

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.CategoryId = dto.CategoryId;
        product.CreatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(product);

        var updatedWithNav = await _productRepository.GetByIdAsync(id);

        return Result<ProductDto>.Success(MapToDto(updatedWithNav!));
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
            return Result.Failure("Produto não encontrado.", 404);

        await _productRepository.DeleteAsync(product);
        return Result.Success();
    }

    private static ProductDto MapToDto(Product p)
    {
        return new ProductDto(
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            p.CategoryId,
            p.Category != null
                ? new CategoryDto(
                    p.Category.Id,
                    p.Category.Name,
                    p.Category.Description,
                    p.Category.CreatedAt,
                    p.Category.UpdatedAt
                )
                : null,
            p.CreatedAt,
            p.UpdatedAt
        );
    }
}
