using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.Categories.Commads;
using TaskManagementSystem.Application.Categories.Queries;
using TaskManagementSystem.Application.Contracts.Category.Request;

namespace TaskManagementSystem.Api.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GellAllCategories()
    {
        var query = new GetAllCategories();
        var response = await _mediator.Send(query);
        if (response.IsError)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var query = new GetCategoryById() { CategoryId = id };
        var resposne = await _mediator.Send(query);
        if (resposne.IsError)
        {
            return BadRequest(resposne);
        }
        return Ok(resposne);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategory request)
    {
        var command = new CreateCategoryCommand()
        {
            Name = request.Name
        };

        var result = await _mediator.Send(command);
        if (result.IsError)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(GetCategory), new { id = result.Payload.Id }, result);
    }


    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategory updateRequest)
    {
        var command = new UpdateCategoryCommand()
        {
            Id = id,
            Name = updateRequest.Name,
        };
        var result = await _mediator.Send(command);
        if (result.IsError)
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var command = new DeleteCategoryCommand() { CategoryId = id };

        var resposne = await _mediator.Send(command);
        if (resposne.IsError)
        {
            return BadRequest();
        }
        return NoContent();

    }
}
