namespace DotNetToolbox.AI.Jobs;

public class UserProfile
    : Context, IValidatable {
    public UserProfile(string id) {
        this[nameof(Id)] = id;
        this[nameof(Name)] = string.Empty;
        this[nameof(Language)] = "English";
        this[nameof(Facts)] = new List<string>();
    }

    public string Id => (string)this[nameof(Id)]!;

    public required string Name {
        get => (string)this[nameof(Name)]!;
        init => this[nameof(Name)] = value;
    }

    public string Language {
        get => (string)this[nameof(Language)]!;
        init => this[nameof(Language)] = value;
    }

    public List<string> Facts => (List<string>)this[nameof(Facts)]!;

    public Result Validate(IDictionary<string, object?>? context = null) => Result.Success();
}
