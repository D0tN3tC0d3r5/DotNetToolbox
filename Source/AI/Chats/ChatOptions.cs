namespace DotNetToolbox.AI.Chats;

public record ChatOptions : IValidatable {
    public virtual Result Validate(IDictionary<string, object?>? context = null) => Result.Success();
}
