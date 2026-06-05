using Microsoft.AspNetCore.Mvc;
using ZdzcStock.Application.DTOs;
using ZdzcStock.Application.Services;

namespace ZdzcStock.Api.Controllers;

[ApiController]
[Route("api/products")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;

    public ProductsController(ProductService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int? categoryId)
    {
        var products = await _service.GetAllAsync(categoryId);
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.IsSuccess)
            return StatusCode(result.ErrorStatusCode!.Value, new { message = result.ErrorMessage });

        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        var result = await _service.CreateAsync(dto);
        if (!result.IsSuccess)
            return StatusCode(result.ErrorStatusCode!.Value, new { message = result.ErrorMessage });

        return CreatedAtAction(nameof(GetAll), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (!result.IsSuccess)
            return StatusCode(result.ErrorStatusCode!.Value, new { message = result.ErrorMessage });

        return Ok(result.Value);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result.IsSuccess)
            return StatusCode(result.ErrorStatusCode!.Value, new { message = result.ErrorMessage });

        return NoContent();
    }
}
