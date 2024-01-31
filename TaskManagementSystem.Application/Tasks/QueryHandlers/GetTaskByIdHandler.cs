using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Contracts.Task.Response;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Application.Tasks.Queries;
using TaskManagementSystem.DAL.Data;

namespace TaskManagementSystem.Application.Tasks.QueryHandlers;
internal class GetTaskByIdHandler : IRequestHandler<GetTaskById, OperationResult<TaskWithCategoryDetailsResponse>>
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    public GetTaskByIdHandler(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<OperationResult<TaskWithCategoryDetailsResponse>> Handle(GetTaskById request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<TaskWithCategoryDetailsResponse>();
        try
        {
            var task = await _dataContext.Tasks
                .Include(t => t.Category)
                .Include(u => u.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);
            if(task is null)
            {
                result.AddError(ErrorCode.NotFound, "No task is found.");
                return result;
            }
            var mappedTask = _mapper.Map<TaskWithCategoryDetailsResponse>(task);
            result.Payload = mappedTask;
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.UnknownError, ex.Message);
        }

        return result;
    }
}
