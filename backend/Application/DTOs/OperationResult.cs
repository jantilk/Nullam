using Microsoft.AspNetCore.Http;

namespace Application.DTOs;

public class OperationResult<T>
{
    public bool IsSuccess { get; init; }
    public T? Value { get; init; }
    public string? Error { get; init; }
    public int StatusCode { get; init; }

    public static OperationResult<T> Success(T value) => new() { IsSuccess = true, Value = value };
    public static OperationResult<T> Failure(string error, int statusCode = StatusCodes.Status400BadRequest) 
        => new() { Error = error, StatusCode = statusCode };
    
    public static OperationResult<T> FailureWithLog(string error)
    {
        // TODO save logs to db

        return new OperationResult<T> {Error = error};
    }
}