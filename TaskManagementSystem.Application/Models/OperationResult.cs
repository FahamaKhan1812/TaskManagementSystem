using TaskManagementSystem.Application.Enums;

namespace TaskManagementSystem.Application.Models;
public class OperationResult<T>
{
    public bool IsError { get; set; }
    public ErrorCode Code { get; set; }
    public T? Payload { get; set; } = default;
    public List<Error> Errors { get; set; } = new List<Error>();

    public OperationResult()
    {
        
    }

    private OperationResult(bool isError, ErrorCode code,T payload, List<Error> errors)
    {
        Code = code;
        IsError = isError;
        Payload = payload;
        Errors = errors;
    }

    private OperationResult(bool isError,ErrorCode code,T payload)
    {
        Code = code;
        IsError = isError;
        Payload = payload;
    }

    public static OperationResult<T> Success(ErrorCode code ,T payload, List<Error> errors)
    {
        return new(false,code, payload, errors);
    }
    public static OperationResult<T> Success(ErrorCode code ,T payload)
    {
        return new(false,code, payload);
    }
    public static OperationResult<T> Failure(ErrorCode code, T payload, List<Error> errors)
    {
        return new(true,code, payload, errors);
    }
    public static OperationResult<T> Failure(ErrorCode code, T payload)
    {
        return new(true,code, payload);
    }

}
