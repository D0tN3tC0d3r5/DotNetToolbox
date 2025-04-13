namespace WebApi.Model;

public enum SignInResult {
    InvalidInput,
    AccountNotFound,
    AccountIsBlocked,
    AccountConfirmationRequired,
    TwoFactorIsNotEnabled,
    SignInIsNotAllowed,
    AccountIsLocked,
    Incorrect,
    TwoFactorRequired,
    Success,
}
