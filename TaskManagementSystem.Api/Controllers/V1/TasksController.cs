using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.Contracts.Task.Request;
using TaskManagementSystem.Application.Tasks.Commands;
using TaskManagementSystem.Application.Tasks.Queries;

namespace TaskManagementSystem.Api.Controllers.V1;

[ApiVersion("1.0")]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class TasksController : BaseController
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks()
    {
        var query = new GetAllTasks();
        var response = await _mediator.Send(query);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        var query = new GetTaskById() { TaskId = id };
        var response = await _mediator.Send(query);
        return response.IsError ? HandleErrorResponse(response.Errors) : Ok(response.Payload);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateTask([FromBody] CreateTask request)
    {
        var command = new CreateTaskCommand()
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            IsCompleted = request.IsCompleted,
            CategoryId = request.CategoryId,
            UserId = request.UserId
        };

        var result = await _mediator.Send(command);
        return result.IsError ? HandleErrorResponse(result.Errors)
            : CreatedAtAction(nameof(GetTaskById), new { id = result.Payload.Id }, result.Payload);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTask request)
    {
        var command = new UpdateTaskCommand()
        {
            Description = request.Description,
            IsCompleted = request.IsCompleted,
            TaskId = id,
            Title = request.Title,
            Priority = request.Priority,
            CategoryId = request.CategoryId,
            UserId = request.UserId
        };

        var result = await _mediator.Send(command);
        return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id, [FromBody] DeleteTask request)
    {
        var command = new DeleteTaskCommand() 
        { 
            TaskId = id,
            UserId = request.UserId
        };
        var resposne = await _mediator.Send(command);
        return resposne.IsError ? HandleErrorResponse(resposne.Errors) : NoContent();
    }
}
