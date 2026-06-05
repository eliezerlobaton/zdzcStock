using Microsoft.AspNetCore.Mvc;
using ZdzcStock.Application.DTOs;
using ZdzcStock.Application.Services;

namespace ZdzcStock.Api.Controllers;

[ApiController]
[Route("api/Categories")]
[Produces("application/json")]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _service;

    public CategoryController(CategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _service.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.IsSuccess)
            return StatusCode(result.ErrorStatusCode!.Value, new { message = result.ErrorMessage });

        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
    {
        var result = await _service.UpdateAsync(id, dto);
        if (!result.IsSuccess)
            return StatusCode(result.ErrorStatusCode!.Value, new { message = result.ErrorMessage });

        return Ok(result.Value);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result.IsSuccess)
            return StatusCode(result.ErrorStatusCode!.Value, new { message = result.ErrorMessage });

        return NoContent();
    }
}
