using Model = DotNetToolbox.AI.Models.Model;

namespace Lola.Models.Repositories;

public class ModelEntity : Entity<ModelEntity, uint> {
    public uint ProviderId { get; set; }
    [JsonIgnore]
    public ProviderEntity? Provider { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public uint MaximumContextSize { get; set; }
    public uint MaximumOutputTokens { get; set; }
    public decimal InputCostPerMillionTokens { get; set; }
    public decimal OutputCostPerMillionTokens { get; set; }
    public DateOnly? TrainingDateCutOff { get; set; }
    public bool Selected { get; set; }

    public override Result Validate(IMap? context = null) {
        var result = base.Validate(context);
        var action = IsNotNull(context).GetRequiredValueAs<EntityAction>(nameof(EntityAction));
        result += action == EntityAction.Insert
                      ? ValidateNewKey(Key, context.GetRequiredValueAs<IModelHandler>(nameof(ModelHandler)))
                      : ValidateKey(Id, Key, context.GetRequiredValueAs<IModelHandler>(nameof(ModelHandler)));
        result += action == EntityAction.Insert
                      ? ValidateNewName(Name, context.GetRequiredValueAs<IModelHandler>(nameof(ModelHandler)))
                      : ValidateName(Id, Name, context.GetRequiredValueAs<IModelHandler>(nameof(ModelHandler)));

        result += ValidateProvider(ProviderId, context.GetRequiredValueAs<IProviderHandler>(nameof(ProviderHandler)));
        result += ValidateInputCost(InputCostPerMillionTokens);
        result += ValidateOutputCost(OutputCostPerMillionTokens);
        result += ValidateDateCutOff(TrainingDateCutOff);
        return result;
    }

    public static Result ValidateNewKey(string? key, IModelHandler handler) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(key))
            result += new ValidationError("The key is required.", nameof(Key));
        else if (handler.Find(m => m.Key.Equals(key, StringComparison.OrdinalIgnoreCase)) is not null)
            result += new ValidationError("A model with this key is already registered.", nameof(Key));
        return result;
    }

    public static Result ValidateKey(uint id, string? key, IModelHandler handler) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(key))
            result += new ValidationError("The key is required.", nameof(Key));
        else if (handler.Find(m => m.Key.Equals(key, StringComparison.OrdinalIgnoreCase) && m.Id != id) is not null)
            result += new ValidationError("A model with this key is already registered.", nameof(Key));
        return result;
    }

    public static Result ValidateNewName(string? name, IModelHandler handler) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(name))
            result += new ValidationError("The name is required.", nameof(Name));
        else if (handler.Find(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) is not null)
            result += new ValidationError("A model with this name is already registered.", nameof(Name));
        return result;
    }

    public static Result ValidateName(uint id, string? name, IModelHandler handler) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(name))
            result += new ValidationError("The name is required.", nameof(Name));
        else if (handler.Find(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && m.Id != id) is not null)
            result += new ValidationError("A model with this name is already registered.", nameof(Name));
        return result;
    }

    public static Result ValidateProvider(uint providerId, IProviderHandler handler) {
        var result = Result.Success();
        if (handler.GetById(providerId) is null)
            result += new ValidationError("The provider does not exist.", nameof(ProviderId));
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

    public static implicit operator Model(ModelEntity entity) => new(entity.Id) {
        Provider = entity.Provider!.Name,
        Key = entity.Key,
        Name = entity.Name,
        MaximumContextSize = entity.MaximumContextSize,
        MaximumOutputTokens = entity.MaximumOutputTokens,
        TrainingDataCutOff = entity.TrainingDateCutOff,
    };
}
