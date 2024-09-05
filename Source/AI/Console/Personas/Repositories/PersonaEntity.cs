namespace AI.Sample.Personas.Repositories;

public class PersonaEntity
    : Entity<PersonaEntity, uint> {
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public List<string> Goals { get; } = [];
    public string Expertise { get; set; } = string.Empty;
    public List<Query> Questions { get; } = [];
    public List<string> Traits { get; } = [];
    public List<string> Important { get; } = [];
    public List<string> Negative { get; } = [];
    public List<string> Other { get; } = [];

    public override Result Validate(IContext? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Name))
            result += new ValidationError("The name is required.", nameof(Name));
        if (string.IsNullOrWhiteSpace(Role))
            result += new ValidationError("The primary role is required.", nameof(Role));
        if (Goals.Count == 0)
            result += new ValidationError("At least one goal is required.", nameof(Goals));
        return result;
    }

    [JsonIgnore]
    public string Prompt {
        get {
            var sb = new StringBuilder();
            sb.AppendLine($"Your name is {Name}.");
            sb.AppendLine($"You are a highly capable and versatile {Role}.");
            switch (IsNotEmpty(Goals).Count) {
                case 1:
                    sb.AppendLine($"The goal of the task is to {Goals[0]}");
                    break;
                default:
                    sb.AppendLine("The goals of the task are:");
                    for (var i = 0; i < Goals.Count; i++) sb.AppendLine($"{i + 1}. {Goals[i]}");
                    break;
            }
            if (!string.IsNullOrWhiteSpace(Expertise)) sb.AppendLine($"You are an expert in {Expertise}.");
            if (Traits.Count != 0) {
                sb.AppendLine();
                sb.AppendLine("## Your Key Traits:");
                foreach (var trait in Traits) sb.AppendLine($"- {trait}");
            }
            if (Important.Count != 0) {
                sb.AppendLine();
                sb.AppendLine("## Important Information:");
                foreach (var info in Important) sb.AppendLine($"**IMPORTANT!** {info}");
            }
            if (Negative.Count != 0) {
                sb.AppendLine();
                sb.AppendLine("## Never:");
                foreach (var neg in Negative) sb.AppendLine($"**NEVER, IN ANY CIRCUMSTANCES!** {neg}");
            }
            if (Other.Count != 0) {
                sb.AppendLine();
                sb.AppendLine("## Additional Information:");
                foreach (var other in Other) sb.AppendLine(other);
            }
            return sb.ToString().TrimEnd();
        }
    }

    public static implicit operator Persona(PersonaEntity entity)
        => new() {
            Name = entity.Name,
            Role = entity.Role,
            Goals = entity.Goals,
            Expertise = entity.Expertise,
            Traits = entity.Traits,
            Important = entity.Important,
            Negative = entity.Negative,
            Other = entity.Other,
        };
}
