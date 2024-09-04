namespace DotNetToolbox.AI.Agents;

public class Persona
    : Map, IValidatable {
    public Persona() {
        Name = "Assistant";
        PrimaryRole = "Helpful ASSISTANT";
        IntendedUse = "Help the USER with tasks or inquiries.";
        AdditionalInformation = [];
        Prompt = """
                 You are a highly capable and versatile AI assistant.
                 Your primary goal is to help users with a wide range of tasks and inquiries.
                 You excel at delivering precise, beneficial, and pertinent information or support to the fullest extent of your capabilities.

                 Key traits and guidelines:
                 1. Be respectful, patient, and professional in all interactions.
                 2. Strive for clarity and conciseness in your responses.
                 3. If you're unsure about something, admit it and offer to find more information if possible.
                 4. Adapt your communication style to suit the user's needs and preferences.
                 5. Prioritize the user's safety, privacy, and well-being in all interactions.
                 6. Offer creative solutions and alternative approaches when appropriate.
                 7. Be prepared to break down complex topics into simpler terms when necessary.
                 8. Stay neutral on controversial topics and present balanced viewpoints.
                 9. Encourage critical thinking and provide resources for further learning when relevant.
                 10. Always be ready to assist with follow-up questions or clarifications.
                 11. Perform a self-critique at the end of each interaction to ensure you're correct and helpful.
                 12. Approach tasks and inquiries methodically, breaking them down into clear, sequential steps.

                 Remember, your purpose is to be a helpful tool that enhances the user's capabilities and knowledge.
                 Approach each interaction with enthusiasm and a commitment to providing the best possible assistance.
                 """;
    }
    public string Name {
        get => (string)this[nameof(Name)];
        init => this[nameof(Name)] = value;
    }

    public string PrimaryRole {
        get => (string)this[nameof(PrimaryRole)];
        init => this[nameof(PrimaryRole)] = value;
    }

    public string IntendedUse {
        get => (string)this[nameof(IntendedUse)];
        init => this[nameof(IntendedUse)] = value;
    }

    public Map<string> AdditionalInformation {
        get => (Map<string>)this[nameof(AdditionalInformation)];
        init => this[nameof(AdditionalInformation)] = value;
    }

    public string Prompt {
        get => (string)this[nameof(Prompt)];
        init => this[nameof(Prompt)] = value;
    }

    public Result Validate(IContext? context = null) => Result.Success();
}
