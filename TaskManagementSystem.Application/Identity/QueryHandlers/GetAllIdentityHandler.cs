using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Contracts.Identity.Response;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Identity.Queries;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.DAL.Data;

namespace TaskManagementSystem.Application.Identity.QueryHandlers;
internal class GetAllIdentityHandler : IRequestHandler<GetAllIdentity, OperationResult<List<IdentityResponse>>>
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public GetAllIdentityHandler(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<OperationResult<List<IdentityResponse>>> Handle(GetAllIdentity request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<IdentityResponse>>();

        try
        {
            if(request.UserRole != "Admin")
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.UserNotAllowed,
                    Message = "User is not allowed to do specific operation"
                };
                result.Errors.Add(error);
                return result;
            }
            var identityUsers = await _dataContext.Users
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            var mappedData = _mapper.Map<List<IdentityResponse>>(identityUsers);
            result.Payload = mappedData;

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
