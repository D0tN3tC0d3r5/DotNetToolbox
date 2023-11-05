namespace DotNetToolbox.Security.Auth;

public interface IRoleId<out TKey> {
    TKey Id { get; }
}