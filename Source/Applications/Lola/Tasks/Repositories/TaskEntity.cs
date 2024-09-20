using Task = DotNetToolbox.AI.Jobs.Task;

namespace Lola.Tasks.Repositories;

public class TaskEntity
    : Entity<TaskEntity, uint> {
    public string Name { get; init; } = string.Empty;
    public List<string> Goals { get; init; } = [];
    public List<string> Scope { get; init; } = [];
    public List<string> Requirements { get; init; } = [];
    public List<string> Assumptions { get; init; } = [];
    public List<string> Constraints { get; init; } = [];
    public List<string> Examples { get; init; } = [];
    public List<string> Guidelines { get; init; } = [];
    public List<string> Validations { get; init; } = [];

    public string InputTemplate { get; init; } = string.Empty;
    public TaskResponseType ResponseType { get; init; }
    public string ResponseSchema { get; init; } = string.Empty;

    public override Result Validate(IMap? context = null) {
        var result = base.Validate(context);
        var action = IsNotNull(context).GetRequiredValueAs<EntityAction>(nameof(EntityAction));
        result += action == EntityAction.Insert
                      ? ValidateNewName(Name, context.GetRequiredValueAs<ITaskHandler>(nameof(TaskHandler)))
                      : ValidateName(Id, Name, context.GetRequiredValueAs<ITaskHandler>(nameof(TaskHandler)));
        if (Goals.Count == 0) result += new ValidationError("At least one goal is required.", nameof(Goals));
        return result;
    }

    public static Result ValidateNewName(string? name, ITaskHandler handler) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(name))
            result += new ValidationError("The name is required.", nameof(Name));
        else if (handler.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) is not null)
            result += new ValidationError("A task with this name is already registered.", nameof(Name));
        return result;
    }

    public static Result ValidateName(uint id, string? name, ITaskHandler handler) {
        var result = Result.Success();
        if (string.IsNullOrWhiteSpace(name))
            result += new ValidationError("The name is required.", nameof(Name));
        else if (handler.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && p.Id != id) is not null)
            result += new ValidationError("A task with this name is already registered.", nameof(Name));
        return result;
    }

    public static implicit operator Task(TaskEntity entity)
        => new(entity.Id) {
            Name = entity.Name,
            Goals = entity.Goals,
            Scope = entity.Scope,
            Requirements = entity.Requirements,
            Assumptions = entity.Assumptions,
            Constraints = entity.Constraints,
            Examples = entity.Examples,
            Guidelines = entity.Guidelines,
            Validations = entity.Validations,
            InputTemplate = entity.InputTemplate,
            ResponseType = entity.ResponseType,
            ResponseSchema = entity.ResponseSchema,
        };
}
