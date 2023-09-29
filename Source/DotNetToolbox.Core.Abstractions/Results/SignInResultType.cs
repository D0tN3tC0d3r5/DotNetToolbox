namespace System.Results;

public enum SignInResultType
{
    ValidationFailure = 0, // request validation failed.
    Blocked = 1, // account is blocked.
    Locked = 2, // account is locked.
    Failure = 3, // failed attempt.
    ConfirmationRequired = 4, // attempt succeeded but email is not confirmed.
    TwoFactorRequired = 5, // attempt succeeded, but requires 2-factor authentication.
    Success = 6, // attempt succeeded.
}
