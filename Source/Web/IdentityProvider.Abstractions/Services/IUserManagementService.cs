namespace WebApi.Services;

public interface IUserManagementService {
    Task<Result<AddUserResponse>> AddUserAsync(AddUserRequest request);

    Task<Result<UserResponse>> FindUserAsync(UserRequest identifier);

    Task<Result> UpdateUserAsync(UpdateUserRequest request);

    Task RemoveUserAsync(UserRequest identifier);

    Task AssignRoleAsync(UserRoleRequest request);

    Task RemoveRoleAsync(UserRoleRequest request);
}
