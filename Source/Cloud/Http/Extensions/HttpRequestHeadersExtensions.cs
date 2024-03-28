using Convert = System.Convert;

namespace DotNetToolbox.Http.Extensions;

public static class HttpRequestHeadersExtensions {
    public static bool TryGetValue<T>(this HttpRequestHeaders headers, string key, out T value) {
        try {
            value = headers.GetValue<T>(key);
            return true;
        }
        catch {
            value = default!;
            return false;
        }
    }

    public static bool TryGetValue(this HttpRequestHeaders headers, string key, out string value) {
        try {
            value = headers.GetValue(key);
            return true;
        }
        catch {
            value = default!;
            return false;
        }
    }

    public static T GetValue<T>(this HttpRequestHeaders headers, string key)
        => (T)ChangeType(headers.GetValue(key), typeof(T));

    public static string GetValue(this HttpRequestHeaders headers, string key)
        => IsNotNull(headers).TryGetValues(IsNotNullOrWhiteSpace(key), out var values)
            ? values.Single()
            : throw new InvalidOperationException($"Header '{key}' not found.");
}
