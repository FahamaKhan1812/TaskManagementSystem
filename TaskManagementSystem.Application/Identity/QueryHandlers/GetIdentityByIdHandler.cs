using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Contracts.Identity.Response;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Identity.Queries;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.DAL.Data;

namespace TaskManagementSystem.Application.Identity.QueryHandlers;
internal class GetIdentityByIdHandler : IRequestHandler<GetIdentityById, OperationResult<IdentityResponse>>
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public GetIdentityByIdHandler(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<OperationResult<IdentityResponse>> Handle(GetIdentityById request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<IdentityResponse>();
        try
        {
            if (request.UserRole != "Admin")
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
            var identity = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserId == request.IdentityId, cancellationToken);
            
            if(identity == null)
            {
                result.IsError = true;
                Error error = new()
                {
                    Code = ErrorCode.IdentityUserNotFound,
                    Message = "No Identity is found"
                };
                result.Errors.Add(error);
                return result;
            }

            var mappedData = _mapper.Map<IdentityResponse>(identity);
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
