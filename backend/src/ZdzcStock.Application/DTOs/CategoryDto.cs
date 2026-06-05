namespace ZdzcStock.Application.DTOs;

public record CategoryDto(
    int Id,
    string Name,
    string? Description,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
