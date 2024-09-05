namespace DotNetToolbox.AI.Jobs;

public class Task
    : Map {

    public Task() {
        Goals = [];
        Scope = [];
        Assumptions = [];
        Requirements = [];
        Guidelines = [];
        Constraints = [];
        Validations = [];
        Examples = [];
    }

    public List<string> Goals {
        get => (List<string>)this[nameof(Goals)];
        init => this[nameof(Goals)] = value;
    }
    public List<string> Scope {
        get => (List<string>)this[nameof(Scope)];
        init => this[nameof(Scope)] = value;
    }
    public List<string> Assumptions {
        get => (List<string>)this[nameof(Assumptions)];
        init => this[nameof(Assumptions)] = value;
    }
    public List<string> Requirements {
        get => (List<string>)this[nameof(Requirements)];
        init => this[nameof(Requirements)] = value;
    }
    public List<string> Guidelines {
        get => (List<string>)this[nameof(Guidelines)];
        init => this[nameof(Guidelines)] = value;
    }
    public List<string> Constraints {
        get => (List<string>)this[nameof(Constraints)];
        init => this[nameof(Constraints)] = value;
    }
    public List<string> Examples {
        get => (List<string>)this[nameof(Examples)];
        init => this[nameof(Examples)] = value;
    }
    public List<string> Validations {
        get => (List<string>)this[nameof(Validations)];
        init => this[nameof(Validations)] = value;
    }

    public string Prompt {
        get {
            var sb = new StringBuilder();
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
}
