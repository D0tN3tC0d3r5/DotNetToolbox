using Model = DotNetToolbox.AI.Models.Model;

namespace Lola.Models.Repositories;

public class ModelEntity : Entity<ModelEntity, string> {
    public uint ProviderKey { get; set; }
    [JsonIgnore]
    public ProviderEntity? Provider { get; set; }
    public string Name { get; set; } = string.Empty;
    public uint MaximumContextSize { get; set; }
    public uint MaximumOutputTokens { get; set; }
    public decimal InputCostPerMillionTokens { get; set; }
    public decimal OutputCostPerMillionTokens { get; set; }
    public DateOnly? TrainingDateCutOff { get; set; }
    public bool Selected { get; set; }

    public override Result Validate(IMap? context = null) {
        var result = base.Validate(context);
        result += ValidateKey(Key, IsNotNull(context).GetRequiredValueAs<IModelHandler>(nameof(ModelHandler)));
        result += ValidateName(Name, IsNotNull(context).GetRequiredValueAs<IModelHandler>(nameof(ModelHandler)));
        result += ValidateInputCost(InputCostPerMillionTokens);
        result += ValidateOutputCost(OutputCostPerMillionTokens);
        result += ValidateDateCutOff(TrainingDateCutOff);
        if (MaximumContextSize == 0)
            result += new ValidationError("MaximumContextSize must be greater than 0.", nameof(MaximumContextSize));
        if (MaximumOutputTokens == 0)
            result += new ValidationError("MaximumOutputTokens must be greater than 0.", nameof(MaximumOutputTokens));
        return result;
    }

    public static Result ValidateKey(string? key, IModelHandler handler) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(key))
            result += new ValidationError("The identifier is required.", nameof(Key));
        else if (handler.GetByKey(key) is not null)
            result += new ValidationError("A model with this identifier is already registered.", nameof(Key));
        return result;
    }

    public static Result ValidateName(string? name, IModelHandler handler) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(name))
            result += new ValidationError("The name is required.", nameof(Name));
        else if (handler.GetByName(name) is not null)
            result += new ValidationError("A model with this name is already registered.", nameof(Name));
        return result;
    }

    public static Result ValidateInputCost(decimal value) {
        var result = Result.Success();
        if (value < 0)
            result += new ValidationError("The input cost per million tokens must be greater than or equal to zero.", nameof(InputCostPerMillionTokens));
        return result;
    }

    public static Result ValidateOutputCost(decimal value) {
        var result = Result.Success();
        if (value < 0)
            result += new ValidationError("The input cost per million tokens must be greater than or equal to zero.", nameof(InputCostPerMillionTokens));
        return result;
    }

    public static Result ValidateDateCutOff(DateOnly? value) {
        var result = Result.Success();
        if (value is not null && value.Value > DateOnly.FromDateTime(DateTime.UtcNow))
            result += new ValidationError("The training data cut off date must be in the past.", nameof(TrainingDateCutOff));
        if (value is not null && value.Value <= DateOnly.Parse("2021-01"))
            result += new ValidationError("The training data cut off date must be after Jan. 2021.", nameof(TrainingDateCutOff));
        return result;
    }

    public static implicit operator Model(ModelEntity entity) => new(entity.Key) {
        Provider = entity.Provider!.Name,
        Name = entity.Name,
        MaximumContextSize = entity.MaximumContextSize,
        MaximumOutputTokens = entity.MaximumOutputTokens,
        TrainingDataCutOff = entity.TrainingDateCutOff,
    };
}
