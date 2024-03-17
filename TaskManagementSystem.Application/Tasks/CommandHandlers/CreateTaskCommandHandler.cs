using AutoMapper;
using MediatR;
using TaskManagementSystem.Application.Contracts.Task.Request;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Application.Tasks.Commands;
using TaskManagementSystem.Domain.Tasks;

namespace TaskManagementSystem.Application.Tasks.CommandHandlers;
public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, OperationResult<CreateTask>>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public CreateTaskCommandHandler(IMapper mapper, ITaskRepository taskRepository)
    {
        _mapper = mapper;
        _taskRepository = taskRepository;
    }

    public async Task<OperationResult<CreateTask>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<CreateTask>();
        try
        {
            Domain.Tasks.Task task = new()
            {
                Id = request.Id,
                Description = request.Description,
                Priority = request.Priority,
                CreatedDate = request.CreatedDate,
                IsCompleted = request.IsCompleted,
                Title = request.Title,
                CategoryId = request.CategoryId,
                UpdatedDate = request.UpdatedDate,
                UserProfileId = request.UserId
            };

            await _taskRepository.AddAsync(task, cancellationToken);

            var mappedTask = _mapper.Map<CreateTask>(task);

            result.Payload = mappedTask;
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.UnknownError, ex.Message);
        }
        return result;
    }
}
