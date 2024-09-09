using Task = DotNetToolbox.AI.Jobs.Task;

namespace AI.Sample.Tasks.Repositories;

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

    public override Result Validate(IContext? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Name)) result += new ValidationError("The name is required.", nameof(Name));
        if (Goals.Count == 0) result += new ValidationError("At least one goal is required.", nameof(Goals));
        return result;
    }

    public static implicit operator Task(TaskEntity entity)
        => new(entity.Key) {
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

