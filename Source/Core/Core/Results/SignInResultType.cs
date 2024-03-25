namespace DotNetToolbox.Results;

[Flags]
public enum SignInResultType : byte {
    Pending = 0, // The sign in was not processed.
    Error = 1, // An internal error has occurred.
    Invalid = 2, // request validation failed. No attempt was made.
    Failed = 4, // attempt failed.
    Blocked = 12, // account is blocked. (8 | 4) Counts as Failed.
    Locked = 20, // account is locked. (16 | 4) Counts as Failed.
    Success = 32, // attempt succeeded.
    ConfirmationPending = 96, // attempt succeeded but email is not confirmed. (64 | 32).
    TwoFactorRequired = 160, // attempt succeeded, but requires 2-factor authentication. (128 | 32).
}
