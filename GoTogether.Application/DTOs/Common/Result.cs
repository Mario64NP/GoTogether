namespace GoTogether.Application.DTOs.Common;

public enum ErrorType
{
    None,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden,
    Failure
}

public record Result(bool IsSuccess, string? Error = null, ErrorType ErrorType = ErrorType.None)
{
    public static Result Success() => new(true);
    public static Result Failure(string error, ErrorType type = ErrorType.Failure)
        => new(false, error, type);
}

public record Result<T>(bool IsSuccess, T? Value = default, string? Error = null, ErrorType ErrorType = ErrorType.None) : Result(IsSuccess, Error, ErrorType)
{
    public static Result<T> Success(T value) => new(true, value, null, ErrorType.None);
    public new static Result<T> Failure(string error, ErrorType type = ErrorType.Failure)
        => new(false, default, error, type);
}