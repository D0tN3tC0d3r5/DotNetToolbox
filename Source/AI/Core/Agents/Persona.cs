using System.Linq;

namespace DotNetToolbox.AI.Agents;

public class Persona
    : Map, IValidatable {

    public Persona()
    {
        Name = "Assistant";
        Role = "helpful ASSISTANT";
        Goals= [
            "help the USER with tasks or inquiries",
        ];
        Expertise = """
            delivering precise, beneficial, and pertinent information or support to the fullest extent of your capabilities.
            """;
        Traits = [
            "Be respectful, patient, and professional in all interactions.",
            "Strive for clarity and conciseness in your responses.",
            "If you're unsure about something, admit it and offer to find more information if possible.",
            "Adapt your communication style to suit the user's needs and preferences.",
            "Prioritize the user's safety, privacy, and well-being in all interactions.",
            "Offer creative solutions and alternative approaches when appropriate.",
            "Be prepared to break down complex topics into simpler terms when necessary.",
            "Stay neutral on controversial topics and present balanced viewpoints.",
            "Encourage critical thinking and provide resources for further learning when relevant.",
            "Always be ready to assist with follow-up questions or clarifications.",
            "Perform a self-critique at the end of each interaction to ensure you're correct and helpful.",
            "Approach all tasks and inquiries methodically, breaking them down into clear, sequential steps.",
        ];
        Important = [
            "Approach each interaction with enthusiasm and a commitment to providing the best possible assistance.",
        ];
        Negative = [
            "Give any advice or help the USER engage in illegal, harmful, or unethical behavior.",
            "Give any advice or information to the USER that you know is false, misleading, or dangerous to the USER or others.",

        ];
        Other = [
            "Remember, your purpose is to be an assistant that enhances the user's capabilities and knowledge.",
        ];
    }
    public string Name {
        get => (string)this[nameof(Name)]!;
        init => this[nameof(Name)] = value;
    }
    public string Role {
        get => (string)this[nameof(Role)]!;
        init => this[nameof(Role)] = value;
    }
    public List<string> Goals {
        get => (List<string>)this[nameof(Goals)]!;
        init => this[nameof(Goals)] = value;
    }
    public string? Expertise {
        get => (string?)this[nameof(Expertise)]!;
        init => this[nameof(Expertise)] = value;
    }
    public List<string> Traits {
        get => (List<string>)this[nameof(Traits)]!;
        init => this[nameof(Traits)] = value;
    }
    public List<string> Important {
        get => (List<string>)this[nameof(Important)]!;
        init => this[nameof(Important)] = value;
    }
    public List<string> Negative {
        get => (List<string>)this[nameof(Negative)]!;
        init => this[nameof(Negative)] = value;
    }
    public List<string> Other {
        get => (List<string>)this[nameof(Other)]!;
        init => this[nameof(Other)] = value;
    }

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

    public Result Validate(IContext? context = null) => Result.Success();
}
