namespace Sophia.WebClient.Services;

public class UserRemoteService(HttpClient httpClient, IUserAccessor userAccessor)
    : IUserRemoteService {
    public async Task<UserData> GetCurrentUserProfile() {
        var userId = userAccessor.Id;
        var profile = await httpClient.GetFromJsonAsync<UserData>($"api/users/{userId}");
        return profile ?? new();
    }
}
