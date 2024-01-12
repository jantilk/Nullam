using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class OperationResult<T>
{
    // TODO: check all getters and setters
    public bool IsSuccess { get; init; }
    public T? Value { get; init; }
    public string? Error { get; init; }
    public int StatusCode { get; init; }

    public static OperationResult<T> Success(T value) => new() { IsSuccess = true, Value = value };
    public static OperationResult<T> Failure(string error, int statusCode = StatusCodes.Status400BadRequest) 
        => new() { Error = error, StatusCode = statusCode };
    
    // TODO
    public static OperationResult<T> FailureWithLog(string error)
    {
        // NUllamLogger.LogError(error);

        return new OperationResult<T> {Error = error};
    }
}