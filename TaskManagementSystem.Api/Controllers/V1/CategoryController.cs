using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Api.Extensions;
using TaskManagementSystem.Application.Categories.Commads;
using TaskManagementSystem.Application.Categories.Queries;
using TaskManagementSystem.Application.Contracts.Category.Request;
using TaskManagementSystem.Application.Tasks.Queries;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CategoryController : BaseController
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
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var query = new GetCategoryById() { CategoryId = id };
        var response = await _mediator.Send(query);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategory request)
    {
        var userRole = HttpContext.GetUserRole();
        var command = new CreateCategoryCommand()
        {
            Name = request.Name,
            UserRole = userRole!
        };
        var result = await _mediator.Send(command);
        return result.IsError ? HandleErrorResponse(result.Errors)
            : CreatedAtAction(nameof(GetCategory), new { id = result.Payload.Id }, result.Payload);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategory updateRequest)
    {
        var userRole = HttpContext.GetUserRole();
        var command = new UpdateCategoryCommand()
        {
            Id = id,
            Name = updateRequest.Name,
            UserRole = userRole!
        };
        var result = await _mediator.Send(command);
        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var userRole = HttpContext.GetUserRole();
        var command = new DeleteCategoryCommand() 
        {
            CategoryId = id,
            UserRole = userRole!
        };
        var resposne = await _mediator.Send(command);
        return resposne.IsError ? HandleErrorResponse(resposne.Errors) : NoContent();
    }
}
