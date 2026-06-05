namespace ZdzcStock.Application.DTOs;

public record ProductDto(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    int CategoryId,
    CategoryDto? Category,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
