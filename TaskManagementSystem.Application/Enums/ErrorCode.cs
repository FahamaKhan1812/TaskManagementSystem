namespace TaskManagementSystem.Application.Enums;
public enum ErrorCode
{
    NotFound = 404,
    ServerError = 500,

    ValidationError = 101,

    Ok = 200,
    IdentityUserAlreadyExists = 201,
    IdentityCreationFailed = 202,
    IdentityUserNotFound = 203,
    IncorrectPassword = 204,

    //Application Error should be in the range of 300 - 399
    UserNotAllowed = 300,
    UnAuthorizedAccountRemoval = 306,

    UnknownError = 999
}
