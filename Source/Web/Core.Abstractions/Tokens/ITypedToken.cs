namespace WebApi.Tokens;

public interface ITypedToken<out TToken>
    where TToken : ITypedToken<TToken> {
    static abstract TToken Create(string value);
}
