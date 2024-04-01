namespace Sophia.WebApp.Components.Account;
internal sealed class IdentityNoOpEmailSender : IEmailSender<User> {
    private readonly NoOpEmailSender _emailSender = new();

    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
        => _emailSender.SendEmailAsync(email, "Confirm your email", $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
        => _emailSender.SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
        => _emailSender.SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");
}
