namespace Sophia.Services;

public class UserService(DataContext dbContext, IUserAccessor userAccessor)
    : IUserService {
    public async Task<UserData> GetCurrentUserProfile() {
        var userId = userAccessor.Id;
        var user = await dbContext.Users.FindByKey(userId);
        return user ?? new();
    }
}
