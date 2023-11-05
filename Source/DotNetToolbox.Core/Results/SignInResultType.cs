namespace System.Results;

public enum SignInResultType {
    Failed = 0, // attempt failed.
    Locked = 1, // account is locked.
    Blocked = 2, // account is blocked.
    ConfirmationRequired = 3, // attempt succeeded but email is not confirmed.
    TwoFactorRequired = 4, // attempt succeeded, but requires 2-factor authentication.
    Success = 5, // attempt succeeded.
}
