namespace DotNetToolbox.Http.Extensions;

public static class ModelStateDictionaryExtensions {
    public static void AddResult(this ModelStateDictionary modelState, ResultBase result) {
        foreach (var error in result.Errors)
            modelState.AddModelError(error.Source, error.FormattedMessage);
    }
}
