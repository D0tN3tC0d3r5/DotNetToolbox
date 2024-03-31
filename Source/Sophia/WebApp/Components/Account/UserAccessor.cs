using Sophia.Models.Users;

using UserData = Sophia.Models.Users.UserData;

namespace Sophia.WebApp.Components.Account;
internal sealed class UserAccessor(UserManager<UserData> userManager, Redirect redirect) {
    public async Task<UserData> GetRequiredUserAsync(HttpContext context) {
        var user = await userManager.GetUserAsync(context.User);

        if (user is null) {
            redirect.ToWithStatus("Account/InvalidUser", $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.", context);
        }

        return user;
    }
}
