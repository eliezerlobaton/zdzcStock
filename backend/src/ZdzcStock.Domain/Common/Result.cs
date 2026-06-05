namespace ZdzcStock.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }
    public int? ErrorStatusCode { get; }

    protected Result(bool isSuccess, string? errorMessage, int? errorStatusCode)
    {
        if (isSuccess && errorMessage != null)
            throw new InvalidOperationException();

        if (!isSuccess && errorMessage == null)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        ErrorStatusCode = errorStatusCode;
    }

    public static Result Success() => new(true, null, null);

    public static Result Failure(string message, int statusCode = 400) =>
        new(false, message, statusCode);
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, string? errorMessage, int? errorStatusCode)
        : base(isSuccess, errorMessage, errorStatusCode)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(true, value, null, null);

    public static new Result<T> Failure(string message, int statusCode = 400) =>
        new(false, default, message, statusCode);
}
