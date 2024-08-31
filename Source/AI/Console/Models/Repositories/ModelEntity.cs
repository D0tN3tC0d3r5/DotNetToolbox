namespace AI.Sample.Models.Repositories;

public class ModelEntity : Entity<ModelEntity, string> {
    public uint ProviderKey { get; set; }
    public string Name { get; set; } = string.Empty;
    public uint MaximumContextSize { get; set; }
    public uint MaximumOutputTokens { get; set; }
    public decimal InputCostPerMillionTokens { get; set; }
    public decimal OutputCostPerMillionTokens { get; set; }
    public DateOnly TrainingDataCutOff { get; set; }

    public override Result Validate(IContext? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Name))
            result += new ValidationError("Name is required.", nameof(Name));
        if (MaximumContextSize == 0)
            result += new ValidationError("MaximumContextSize must be greater than 0.", nameof(MaximumContextSize));
        if (MaximumOutputTokens == 0)
            result += new ValidationError("MaximumOutputTokens must be greater than 0.", nameof(MaximumOutputTokens));
        return result;
    }
}
