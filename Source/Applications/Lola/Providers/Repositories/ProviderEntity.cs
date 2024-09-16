namespace Lola.Providers.Repositories;

public class ProviderEntity
    : Entity<ProviderEntity, uint> {
    public string Name { get; set; } = string.Empty;
    public string? ApiKey { get; set; }

    public override Result Validate(IMap? context = null) {
        var result = base.Validate(context);
        result += ValidateName(Name, IsNotNull(context).GetRequiredValueAs<IProviderHandler>(nameof(ProviderHandler)));
        return result;
    }

    public static Result ValidateName(string? name, IProviderHandler handler) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(name))
            result += new ValidationError("The name is required.", nameof(Name));
        else if (handler.GetByName(name) is not null)
            result += new ValidationError("A provider with this name is already registered.", nameof(Name));
        return result;
    }

    public static Result ValidateApiKey(string? apiKey) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(apiKey))
            result += new ValidationError("The API Key is required.", nameof(Name));
        return result;
    }
}
