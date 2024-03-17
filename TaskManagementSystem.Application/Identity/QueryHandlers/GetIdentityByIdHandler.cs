using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using TaskManagementSystem.Application.Contracts.Identity.Response;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Identity.Queries;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Domain.Commons;
using TaskManagementSystem.Domain.Users;

namespace TaskManagementSystem.Application.Identity.QueryHandlers;
internal class GetIdentityByIdHandler : IRequestHandler<GetIdentityById, OperationResult<IdentityResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetIdentityByIdHandler(IMapper mapper, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<OperationResult<IdentityResponse>> Handle(GetIdentityById request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<IdentityResponse>();
        try
        {
            if (request.UserRole != UserRole.Admin)
            {
                result.AddError(ErrorCode.UserNotAllowed, "User is not allowed to do specific operation");
                return result;
            }
            var identity = await _userRepository.GetAsync(new Expression<Func<User, bool>>[] { u => u.UserId == request.IdentityId }, cancellationToken);

            if (identity == null)
            {
                result.AddError(ErrorCode.IdentityUserNotFound, "No Identity is found");
                return result;
            }

            var mappedData = _mapper.Map<IdentityResponse>(identity);
            result.Payload = mappedData;
        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.UnknownError, ex.Message);
        }
        return result;
    }
}
