namespace Sophia.WebApp.Services;

public class UserService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    : IUserService {
    public async Task<UserProfileData> GetCurrentUserProfile() {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await dbContext.Users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == userId);
        return user?.Profile.ToDto() ?? new();
    }
}
