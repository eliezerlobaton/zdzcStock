using FluentAssertions;
using Moq;
using ZdzcStock.Application.DTOs;
using ZdzcStock.Application.Services;
using ZdzcStock.Domain.Entities;
using ZdzcStock.Domain.Interfaces;

namespace ZdzcStock.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _service = new ProductService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfProductDtoWithNestedCategory()
    {
        var category = new Category { Id = 1, Name = "Eletrônicos" };
        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Mouse",
                Price = 100,
                CategoryId = 1,
                Category = category,
            },
        };
        _repositoryMock.Setup(r => r.GetAllWithCategoryAsync()).ReturnsAsync(products);

        var result = await _service.GetAllAsync();

        result.Should().HaveCount(1);
        var dto = result.First();
        dto.Name.Should().Be("Mouse");
        dto.Category.Should().NotBeNull();
        dto.Category!.Name.Should().Be("Eletrônicos");
    }

    [Fact]
    public async Task CreateAsync_WithInvalidCategory_ShouldReturnFailure400()
    {
        var dto = new CreateProductDto("Mouse", "Óptico", 100, 99);
        _repositoryMock.Setup(r => r.CategoryExistsAsync(99)).ReturnsAsync(false);

        var result = await _service.CreateAsync(dto);

        result.IsSuccess.Should().BeFalse();
        result.ErrorStatusCode.Should().Be(400);
        result.ErrorMessage.Should().Be("Categoria informada não existe.");
        _repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistentProduct_ShouldReturnFailure404()
    {
        var dto = new UpdateProductDto("Teclado", null, 150, 1);
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

        var result = await _service.UpdateAsync(1, dto);

        result.IsSuccess.Should().BeFalse();
        result.ErrorStatusCode.Should().Be(404);
        result.ErrorMessage.Should().Be("Produto não encontrado.");
    }
}
