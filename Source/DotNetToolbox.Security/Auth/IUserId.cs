namespace DotNetToolbox.Security.Auth;

public interface IUserId<out TKey> {
    TKey Id { get; }
}