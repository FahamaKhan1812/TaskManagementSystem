using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Contracts.Task.Response;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Application.Tasks.Queries;
using TaskManagementSystem.DAL.Data;

namespace TaskManagementSystem.Application.Tasks.QueryHandlers;
public class GetAllTasksHandler : IRequestHandler<GetAllTasks, OperationResult<List<TaskWithCategoryDetailsResponse>>>
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public GetAllTasksHandler(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<OperationResult<List<TaskWithCategoryDetailsResponse>>> Handle(GetAllTasks request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<TaskWithCategoryDetailsResponse>>();

        try
        {
            var tasksWithCategoryName = await _dataContext.Tasks
                    .Include(t => t.Category) // Include the Category navigation property
                    .Include(u => u.User)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

            result.Payload = _mapper.Map<List<TaskWithCategoryDetailsResponse>>(tasksWithCategoryName);
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
