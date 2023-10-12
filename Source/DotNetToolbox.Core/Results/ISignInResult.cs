namespace System.Results;

public interface ISignInResult : IResult<SignInResultType> {
    string? Token { get; }

    bool IsLocked { get; }
    bool IsBlocked { get; }
    bool IsFailure { get; }
    bool IsConfirmationRequired { get; }
    bool IsTwoFactorRequired { get; }
    bool IsSuccess { get; }
}
