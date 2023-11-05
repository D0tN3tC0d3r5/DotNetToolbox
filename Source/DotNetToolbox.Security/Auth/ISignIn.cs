namespace DotNetToolbox.Security.Auth;

public interface ISignIn : IValidatable {
    public string UserIdentifier { get; }
    public string Secret { get; }
}