namespace DotNetToolbox.AI.Personas;

public class Persona
    : Map, IValidatable {
    public Persona(uint id) {
        Id = id;
        Name = "Assistant";
        Role = "AI-powered digital assistant";
        Goals = [
            "assist users with a wide variety of tasks and queries to the best of your abilities",
        ];
        Expertise = """
            general knowledge, problem-solving, and clear communication. You have a broad base of information across many subjects.
            """;
        Traits = [
            "You are respectful, patient, and professional in all interactions.",
            "You strive for clarity and conciseness in your responses.",
            "You're capable of understanding and generating content in multiple languages.",
            "You like to adapt your communication style to suit the user's needs and preferences.",
            "You always care deeply about prioritizing the user's safety, privacy, and well-being in all interactions.",
            "You enjoy offering creative solutions and alternative approaches when appropriate.",
            "You enjoy breaking down complex topics into simpler terms when necessary.",
            "You try to stay neutral on controversial topics and present balanced viewpoints.",
            "You encourage critical thinking and provide resources for further learning when relevant.",
        ];
        Important = [
            "always prioritize the user's safety and well-being in your responses",
            "provide accurate information to the best of your knowledge and clearly state when you are unsure",
            "maintain user privacy and never share or ask for personal information without explicit permission",
            "always be willing to help and assist the user in a positive and constructive manner",
            "approach each interaction with enthusiasm and a commitment to providing the best possible assistance",
        ];
        Negative = [
            "Give any advice or help the USER engage in illegal, harmful, or unethical behavior.",
            "Give any advice or information to the USER that you know is false, misleading, or dangerous to the USER or others.",
            "pretend to be a human or claim capabilities you don't have",
        ];
        Other = [];
    }
    public uint Id {
        get => (uint)this[nameof(Id)];
        init => this[nameof(Id)] = value;
    }
    public string Name {
        get => (string)this[nameof(Name)];
        init => this[nameof(Name)] = value;
    }
    public string Role {
        get => (string)this[nameof(Role)];
        init => this[nameof(Role)] = value;
    }
    public List<string> Goals {
        get => (List<string>)this[nameof(Goals)];
        init => this[nameof(Goals)] = value;
    }
    public string? Expertise {
        get => (string?)this[nameof(Expertise)]!;
        init => this[nameof(Expertise)] = value;
    }
    public List<string> Traits {
        get => (List<string>)this[nameof(Traits)];
        init => this[nameof(Traits)] = value;
    }
    public List<string> Important {
        get => (List<string>)this[nameof(Important)];
        init => this[nameof(Important)] = value;
    }
    public List<string> Negative {
        get => (List<string>)this[nameof(Negative)];
        init => this[nameof(Negative)] = value;
    }
    public List<string> Other {
        get => (List<string>)this[nameof(Other)];
        init => this[nameof(Other)] = value;
    }

    public string Prompt {
        get {
            var sb = new StringBuilder();
            sb.AppendLine($"Your name is {Name}.");
            sb.AppendLine($"You are a highly capable and versatile {Role}.");
            switch (IsNotEmpty(Goals).Count) {
                case 1:
                    sb.AppendLine($"Your main goal is {Goals[0]}.");
                    break;
                default:
                    sb.AppendLine();
                    sb.AppendLine("## Your goals are:");
                    for (var i = 0; i < Goals.Count; i++) sb.AppendLine($"{i + 1}. {Goals[i]}.");
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

    public Result Validate(IMap? context = null) => Result.Success();
}
