using FluentAssertions;
using Moq;
using ZdzcStock.Application.DTOs;
using ZdzcStock.Application.Services;
using ZdzcStock.Domain.Entities;
using ZdzcStock.Domain.Interfaces;

namespace ZdzcStock.Tests.Services;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _repositoryMock;
    private readonly CategoryService _service;

    public CategoryServiceTests()
    {
        _repositoryMock = new Mock<ICategoryRepository>();
        _service = new CategoryService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfCategoryDto()
    {
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Eletrônicos" },
            new Category { Id = 2, Name = "Móveis" },
        };
        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Eletrônicos");
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateAndReturnDto()
    {
        var dto = new CreateCategoryDto("Livros", "Categoria de livros");
        var categoryCreated = new Category
        {
            Id = 1,
            Name = "Livros",
            Description = "Categoria de livros",
        };

        _repositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Category>()))
            .ReturnsAsync(categoryCreated);

        var result = await _service.CreateAsync(dto);

        result.Id.Should().Be(1);
        result.Name.Should().Be("Livros");
        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Category>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistentId_ShouldReturnFailure404()
    {
        var dto = new UpdateCategoryDto("Novo Nome", null);
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Category?)null);

        var result = await _service.UpdateAsync(99, dto);

        result.IsSuccess.Should().BeFalse();
        result.ErrorStatusCode.Should().Be(404);
        result.ErrorMessage.Should().Be("Categoria não encontrada.");
    }

    [Fact]
    public async Task DeleteAsync_WithLinkedProducts_ShouldReturnFailure409()
    {
        var category = new Category { Id = 1, Name = "Eletrônicos" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
        _repositoryMock.Setup(r => r.HasProductsAsync(1)).ReturnsAsync(true);

        var result = await _service.DeleteAsync(1);

        result.IsSuccess.Should().BeFalse();
        result.ErrorStatusCode.Should().Be(409);

        result
            .ErrorMessage.Should()
            .Be("Não é possível excluir uma categoria que possua produtos vinculados.");

        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WithoutLinkedProducts_ShouldDeleteAndReturnSuccess()
    {
        var category = new Category { Id = 1, Name = "Livros" };
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
        _repositoryMock.Setup(r => r.HasProductsAsync(1)).ReturnsAsync(false);

        var result = await _service.DeleteAsync(1);

        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.DeleteAsync(category), Times.Once);
    }
}
