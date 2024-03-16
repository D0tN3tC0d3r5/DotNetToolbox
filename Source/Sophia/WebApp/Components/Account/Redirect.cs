namespace Sophia.WebApp.Components.Account;
internal sealed class Redirect(NavigationManager navigationManager) {
    public const string StatusCookieName = "Identity.StatusMessage";

    private static readonly CookieBuilder _statusCookieBuilder = new() {
        SameSite = SameSiteMode.Strict,
        HttpOnly = true,
        IsEssential = true,
        MaxAge = TimeSpan.FromSeconds(5),
    };

    [DoesNotReturn]
    public void To(string? uri) {
        uri ??= "";

        // Prevent open redirects.
        if (!Uri.IsWellFormedUriString(uri, UriKind.Relative)) {
            uri = navigationManager.ToBaseRelativePath(uri);
        }

        // During static rendering, NavigateTo throws a NavigationException which is handled by the framework as a redirect.
        // So as long as this is called from a statically rendered Identity component, the InvalidOperationException is never thrown.
        navigationManager.NavigateTo(uri);
        throw new InvalidOperationException($"{nameof(Redirect)} can only be used during static rendering.");
    }

    [DoesNotReturn]
    public void To(string uri, Dictionary<string, object?> queryParameters) {
        var uriWithoutQuery = navigationManager.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);
        var newUri = navigationManager.GetUriWithQueryParameters(uriWithoutQuery, queryParameters);
        To(newUri);
    }

    [DoesNotReturn]
    public void ToWithStatus(string uri, string message, HttpContext context) {
        context.Response.Cookies.Append(StatusCookieName, message, _statusCookieBuilder.Build(context));
        To(uri);
    }

    private string CurrentPath => navigationManager.ToAbsoluteUri(navigationManager.Uri).GetLeftPart(UriPartial.Path);

    [DoesNotReturn]
    public void ToCurrentPage() => To(CurrentPath);

    [DoesNotReturn]
    public void ToCurrentPageWithStatus(string message, HttpContext context)
        => ToWithStatus(CurrentPath, message, context);
}
