namespace WebApi.Endpoints;

public static class IdentityApiPaths {
    public static class Authentication {
        public const string AuthenticationPrefix = "/auth";
        public const string SignIn = "/sign-in";
        public const string SignOut = "/sign-out";
        public const string GetAuthenticationSchemes = "/schemes";
    }

    public static class UserManagement {
        public const string UsersPrefix = "/users";
    }
}
