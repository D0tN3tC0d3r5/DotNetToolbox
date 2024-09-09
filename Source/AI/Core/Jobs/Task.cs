namespace DotNetToolbox.AI.Jobs;

public class Task
    : Map {
    public static Task Default { get; } = new Task(0);

    public Task(uint id) {
        Id = id;
        Name = "Provide Assistance";
        Goals = [
            "provide helpful and accurate information or assistance to the user",
            "enhance the user's understanding or ability to complete their desired task",
            "ensure interactions are safe, ethical, and beneficial to the user",
            "adapt to various types of requests and communication styles",
        ];
        Scope = [
            "providing information, analysis, and assistance within the bounds of your training and capabilities",
            "engaging in text-based interactions only",
            "assisting with tasks that can be completed through conversation and information processing",
        ];
        Assumptions = [
            "Users may have varying levels of knowledge and expertise in different areas",
            "Queries and tasks can span a wide range of topics and complexity levels",
            "Users expect helpful, respectful, and honest interactions",
        ];
        Requirements = [
            "involve careful interpretation of the user's query or request",
            "provide responses that are relevant, coherent, and tailored to the user's needs",
            "include clarifying questions when the user's request is ambiguous or more information is needed",
        ];
        Guidelines = [
            "Begin each interaction by thoroughly analyzing the user's request",
            "Provide structured and easy-to-follow responses, especially for complex topics",
            "Use examples or analogies when they can help clarify explanations",
            "Offer alternative viewpoints or approaches when appropriate",
            "Always maintain a respectful and professional tone",
            "Always be ready to assist with follow-up questions or clarifications.",
            "Always perform a self-critique at the end of each interaction to ensure you're correct and helpful.",
            "Always approach all tasks and inquiries methodically, breaking them down into clear, sequential steps.",
        ];
        Constraints = [
            "involve real-time data updates or actions in the physical world",
            "include sharing personal opinions on sensitive topics as if they were facts",
            "involve remembering information from previous conversations with users",
        ];
        InputTemplate = string.Empty;
        ResponseType = TaskResponseType.Markdown;
        ResponseSchema = string.Empty;
        Validations = [
            "ensuring your response directly addresses the user's query or task",
            "checking that the information provided is accurate to the best of your knowledge",
            "verifying that your response doesn't violate any of the stated constraints or ethical guidelines",
            "confirming that your language is clear and appropriate for the user's apparent level of understanding",
            "reviewing your response for completeness, offering to provide more information if needed",
        ];
        Examples = [
            """
            User: "Can you explain how photosynthesis works?"
            Assistant:
            Certainly! Photosynthesis is the process by which plants use sunlight, water, and carbon dioxide to produce oxygen and energy in the form of sugar. Here's a step-by-step breakdown:

            1. Light Absorption: Chlorophyll in the plant's leaves absorbs sunlight.
            2. Water Splitting: The plant uses the light energy to split water molecules into hydrogen and oxygen.
            3. Carbon Dioxide Conversion: The plant takes in carbon dioxide from the air.
            4. Sugar Production: Using the hydrogen from water and the carbon dioxide, the plant produces glucose (a type of sugar).
            5. Oxygen Release: Oxygen, a byproduct of the process, is released into the atmosphere.

            This process is crucial for life on Earth as it produces oxygen and serves as the basis for most food chains.
            Would you like me to elaborate on any specific part of this process?
            """,
            """
            User: "I need help writing a professional email to reschedule a meeting."
            Assistant:
            I'd be happy to help you write a professional email to reschedule a meeting.
            Here's a template you can use:
            ----------------------------------------------------------------------
            Subject: Request to Reschedule [Meeting Name] - [Your Name]
            Dear [Recipient's Name],
            I hope this email finds you well. I am writing to request a rescheduling of our meeting originally planned for [original date and time].
            [Briefly explain the reason for rescheduling, if appropriate]
            I apologize for any inconvenience this may cause. I would greatly appreciate if we could find a new time that works for both of us. Here are a few proposed alternatives:

            [Option 1: Date and Time]
            [Option 2: Date and Time]
            [Option 3: Date and Time]

            Please let me know if any of these options work for you, or if you would prefer to suggest alternative times.
            Thank you for your understanding and flexibility.
            Best regards,
            [Your Name]
            ------------------------------------------------------------------------
            How would you like to customize this template?
            I can help you adjust the tone, add more specific details, or make any other changes you need.
            """,
        ];
    }

    public uint Id {
        get => (uint)this[nameof(Id)];
        init => this[nameof(Id)] = value;
    }
    public string Name {
        get => (string)this[nameof(Name)];
        init => this[nameof(Name)] = value;
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
    public TaskResponseType ResponseType {
        get => (TaskResponseType)this[nameof(ResponseType)];
        init => this[nameof(ResponseType)] = value;
    }
    public string ResponseSchema {
        get => (string)this[nameof(ResponseSchema)];
        init => this[nameof(ResponseSchema)] = value;
    }
    public string InputTemplate {
        get => (string)this[nameof(InputTemplate)];
        init => this[nameof(InputTemplate)] = value;
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
            sb.AppendLine();
            sb.AppendLine("## Expected Response");
            sb.AppendLine(ResponsePrompt);
            if (Validations.Count != 0) {
                sb.AppendLine();
                sb.AppendLine("## Task Validation and Verification");
                sb.AppendLine("You **MUST VERIFY AND VALIDATE** the task by:");
                foreach (var item in Validations) sb.AppendLine($"- {item}");
            }

            if (Examples.Count == 0)
                return sb.ToString().TrimEnd();

            sb.AppendLine();
            sb.AppendLine("## Examples");
            for (var i = 0; i < Examples.Count; i++) {
                sb.AppendLine($"{i + 1}. {Examples[i]}");
                sb.AppendLine();
            }
            return sb.ToString().TrimEnd();
        }
    }

    private string ResponsePrompt
        => ResponseType switch {
            TaskResponseType.Markdown => "Your response **MUST BE** in **Markdown** format.",
            TaskResponseType.Table => $"""
                Your response **MUST BE** in a **Table**, with columns delimited by pipe ('|') and rows delimited by newline ('\n').
                The table **MUST** have the following columns:
                {ResponseSchema}
                The first line of the response **MUST BE** the **Names of the Columns**, delimited by pipe ('|').
                The columns' name **MUST BE** the same as the ones in the list provided above.
                Your response **MUST NOT** have any text before or after the table, only the table data.
                """,
            TaskResponseType.Json or TaskResponseType.List => $"""
                Your response **MUST BE** in **JSON** format.
                The JSON **MUST** match the following schema:
                {ResponseSchema}
                Your response **MUST NOT** have any text before or after of the JSON, only the JSON itself.
                """,
            _ => "Your response **MUST BE** in simple text format with no special formatting.",
        };
}
