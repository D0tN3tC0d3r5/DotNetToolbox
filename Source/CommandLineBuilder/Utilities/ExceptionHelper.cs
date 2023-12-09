namespace DotNetToolbox.CommandLineBuilder.Utilities;

internal static class ExceptionHelper {
    public static InvalidCastException CreateGetCastException<TTarget>(Argument token) {
        var tokenName = $"{token.TokenType.ToString().ToLower()} '{token.Name}'";
        var sourceType = $"{token.ValueType}{(token is Options ? "[]" : string.Empty)}";
        var targetType = $"{typeof(TTarget).Name}{(token is Options ? "[]" : string.Empty)}";
        return new($"Cannot get {targetType} from {tokenName} ({sourceType}).");
    }
}
