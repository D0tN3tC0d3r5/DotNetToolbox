namespace AI.Sample.Providers.Model;

public class ProviderEntity
    : Entity<ProviderEntity, uint> {
    public string Name { get; set; }

    public override Result Validate(IContext? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Name)) result += new ValidationError("Name is required.", nameof(Name));
        return result;
    }
}
