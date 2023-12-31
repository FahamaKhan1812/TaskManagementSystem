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
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.NotFound,
                    Message = "No task is found."
                };
                result.Errors.Add(error);
                return result;
            }
            var mappedTask = _mapper.Map<TaskWithCategoryDetailsResponse>(task);
            result.Payload = mappedTask;
        }
        catch (Exception ex)
        {
            result.IsError = true;
            Error erros = new()
            {
                Code = ErrorCode.UnknownError,
                Message = ex.Message
            };

            result.Errors.Add(erros);
        }

        return result;
    }
}
