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

    public override Result Validate(IContext? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Name)) result += new ValidationError("The name is required.", nameof(Name));
        if (Goals.Count == 0) result += new ValidationError("At least one goal is required.", nameof(Goals));
        return result;
    }

    [JsonIgnore]
    public string Prompt {
        get {
            var sb = new StringBuilder();
            sb.AppendLine($"## Task: {Name}");
            switch (IsNotEmpty(Goals).Count) {
                case 1:
                    sb.AppendLine($"Your task is to {Goals[0]}");
                    break;
                default:
                    sb.AppendLine("Your task is to:");
                    for (var i = 0; i < Goals.Count; i++) sb.AppendLine($"{i + 1}. {Goals[i]}");
                    break;
            }
            if (Scope.Count != 0) {
                sb.AppendLine();
                sb.AppendLine("## Task Scope");
                sb.AppendLine("The task should be limited to:");
                foreach (var item in Scope) sb.AppendLine($"- {item}");
            }
            if (Assumptions.Count != 0) {
                sb.AppendLine();
                sb.AppendLine("## Task Assumptions");
                sb.AppendLine("The task assumes that:");
                foreach (var item in Assumptions) sb.AppendLine($"- {item}");
            }
            if (Requirements.Count != 0) {
                sb.AppendLine();
                sb.AppendLine("## Task Requirements");
                foreach (var item in Requirements) sb.AppendLine($"- The task **MUST** {item}");
            }
            if (Constraints.Count != 0) {
                sb.AppendLine();
                sb.AppendLine("## Task Constraints");
                foreach (var item in Constraints) sb.AppendLine($"- The task **MUST NOT** {item}");
            }
            if (Guidelines.Count != 0) {
                sb.AppendLine();
                sb.AppendLine("## Task Strategy and Guidelines");
                foreach (var item in Guidelines) sb.AppendLine($"- {item}");
            }
            if (Validations.Count != 0) {
                sb.AppendLine();
                sb.AppendLine("## Task Validation and Verification");
                sb.AppendLine("You **MUST VERIFY AND VALIDATE** the task by:");
                foreach (var item in Validations) sb.AppendLine($"- {item}");
            }
            if (Examples.Count != 0) {
                sb.AppendLine();
                sb.AppendLine("## Examples");
                for (var i = 0; i < Examples.Count; i++) sb.AppendLine($"{i + 1}. {Examples[i]}");
            }
            return sb.ToString().TrimEnd();
        }
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
        };
}
