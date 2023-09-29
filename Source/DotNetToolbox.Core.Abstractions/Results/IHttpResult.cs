namespace System.Results;

public interface ICreateHttpResult
{
    static abstract IHttpResult BadRequest([StringSyntax(CompositeFormat)] string message, params object?[] args);
    static abstract IHttpResult BadRequest(IValidationResult result);
    static abstract IHttpResult Ok();
    static abstract IHttpResult Created();
    static abstract IHttpResult Unauthorized();
    static abstract IHttpResult NotFound();
    static abstract IHttpResult Conflict();

    static abstract IHttpResult<TValue> BadRequestFor<TValue>([StringSyntax(CompositeFormat)] string message, params object?[] args);
    static abstract IHttpResult<TValue> BadRequestFor<TValue>(IValidationResult result);
    static abstract IHttpResult<TValue> OkFor<TValue>(TValue value);
    static abstract IHttpResult<TValue> CreatedFor<TValue>(TValue value);
    static abstract IHttpResult<TValue> UnauthorizedFor<TValue>();
    static abstract IHttpResult<TValue> NotFoundFor<TValue>();
    static abstract IHttpResult<TValue> ConflictWith<TValue>(TValue? value);
}

public interface IHttpResult : IResult<HttpResultType>
{
    bool IsBadRequest { get; }
    bool IsOk { get; }
    bool IsCreated { get; }
    bool IsUnauthorized { get; }
    bool IsNotFound { get; }
    bool IsConflict { get; }
}

public interface IHttpResult<out TValue> : IHttpResult
{
    TValue? Value { get; }

    IHttpResult<TNewValue> Map<TNewValue>(Func<TValue, TNewValue> map);
}
