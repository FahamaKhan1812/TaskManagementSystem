using AutoMapper;
using MediatR;
using TaskManagementSystem.Application.Contracts.Identity.Response;
using TaskManagementSystem.Application.Enums;
using TaskManagementSystem.Application.Identity.Queries;
using TaskManagementSystem.Application.Models;
using TaskManagementSystem.Domain.Commons;
using TaskManagementSystem.Domain.Users;

namespace TaskManagementSystem.Application.Identity.QueryHandlers;

internal sealed class GetAllIdentityHandler : IRequestHandler<GetAllIdentity, OperationResult<List<IdentityResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetAllIdentityHandler(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<OperationResult<List<IdentityResponse>>> Handle(GetAllIdentity request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<IdentityResponse>>();

        try
        {
            if (request.UserRole != UserRole.Admin)
            {
                result.AddError(ErrorCode.UserNotAllowed, "User is not allowed to do specific operation");
                return result;
            }
            var identityUsers = await _userRepository.GetAllAsync(cancellationToken);
            var mappedData = _mapper.Map<List<IdentityResponse>>(identityUsers);
            result.Payload = mappedData;

        }
        catch (Exception ex)
        {
            result.AddError(ErrorCode.UnknownError, ex.Message);
        }
        return result;
    }
}
