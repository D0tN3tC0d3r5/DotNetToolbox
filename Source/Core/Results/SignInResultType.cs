namespace DotNetToolbox.Results;

#pragma warning disable RCS1157
[Flags]
[SuppressMessage("Roslynator", "RCS1135:Declare enum member with zero value (when enum has FlagsAttribute)", Justification = "<Pending>")]
public enum SignInResultType : byte {
    Error = 1, // An internal error has occurred.
    Invalid = 2, // request validation failed. No attempt was made.
    Failed = 4, // attempt failed.
    Blocked = 12, // account is blocked. (8 | 4) Counts as Failed.
    Locked = 20, // account is locked. (16 | 4) Counts as Failed.
    Success = 32, // attempt succeeded.
    ConfirmationPending = 96, // attempt succeeded but email is not confirmed. (64 | 32).
    TwoFactorRequired = 160, // attempt succeeded, but requires 2-factor authentication. (128 | 32).
}
#pragma warning restore RCS1157
