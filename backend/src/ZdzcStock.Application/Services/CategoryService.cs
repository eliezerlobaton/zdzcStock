using ZdzcStock.Application.DTOs;
using ZdzcStock.Domain.Common;
using ZdzcStock.Domain.Entities;
using ZdzcStock.Domain.Interfaces;

namespace ZdzcStock.Application.Services;

public class CategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();
        return categories.Select(c => new CategoryDto(
            c.Id,
            c.Name,
            c.Description,
            c.CreatedAt,
            c.UpdatedAt
        ));
    }

    public async Task<Result<CategoryDto>> GetByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null)
            return Result<CategoryDto>.Failure("Categoria não encontrada", 404);

        return Result<CategoryDto>.Success(
            new CategoryDto(
                category.Id,
                category.Name,
                category.Description,
                category.CreatedAt,
                category.UpdatedAt
            )
        );
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        var created = await _repository.CreateAsync(category);

        return new CategoryDto(
            created.Id,
            created.Name,
            created.Description,
            created.CreatedAt,
            created.UpdatedAt
        );
    }

    public async Task<Result<CategoryDto>> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null)
            return Result<CategoryDto>.Failure("Categoria não encontrada.", 404);

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(category);

        return Result<CategoryDto>.Success(
            new CategoryDto(
                updated.Id,
                updated.Name,
                updated.Description,
                updated.CreatedAt,
                updated.UpdatedAt
            )
        );
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null)
            return Result.Failure("Categria não encontrada.", 404);
        var hasProducts = await _repository.HasProductsAsync(id);
        if (hasProducts)
            return Result.Failure(
                "Não é possível excluir uma categoria que possua produtos vinculados.",
                409
            );

        await _repository.DeleteAsync(category);
        return Result.Success();
    }
}
