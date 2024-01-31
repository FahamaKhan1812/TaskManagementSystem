using AutoMapper;
using MediatR;
using TaskManagementSystem.Application.Contracts.Task.Request;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Application.Tasks.Commands;
using TaskManagementSystem.DAL.Data;

namespace TaskManagementSystem.Application.Tasks.CommandHandlers;
public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, OperationResult<CreateTask>>
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public CreateTaskCommandHandler(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<OperationResult<CreateTask>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<CreateTask>();
        try
        {
            Domain.Entities.Task task = new()
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
            await _dataContext.Tasks.AddAsync(task, cancellationToken);
            await _dataContext.SaveChangesAsync(cancellationToken);

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
