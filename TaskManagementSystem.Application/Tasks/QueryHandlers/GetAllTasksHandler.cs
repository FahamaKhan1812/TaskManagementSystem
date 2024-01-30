using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Contracts.Task.Response;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Application.Tasks.Queries;
using TaskManagementSystem.DAL.Data;

namespace TaskManagementSystem.Application.Tasks.QueryHandlers;
public class GetAllTasksHandler : IRequestHandler<GetAllTasks, OperationResult<TaskResponse>>
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public GetAllTasksHandler(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<OperationResult<TaskResponse>> Handle(GetAllTasks request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<TaskResponse>();

        try
        {
            var query = _dataContext.Tasks.AsQueryable();
            var tasksWithCategoryName = await query
                    .OrderBy(t => t.Id)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(t => t.Category) // Include the Category navigation property
                    .Include(u => u.User)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

            var mappedData = _mapper.Map<List<TaskWithCategoryDetailsResponse>>(tasksWithCategoryName);

            var pageCount = Math.Ceiling(await query.CountAsync(cancellationToken) / (float)request.PageSize);
            TaskResponse obj = new()
            {
                Pages = (int)pageCount,
                CurrentPage = request.Page,
                Tasks = mappedData
            };
            result.Payload = obj;
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
