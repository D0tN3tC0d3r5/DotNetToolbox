namespace DotNetToolbox.AI.Jobs;

public class UserProfile
    : Map,
      IValidatable {
    public UserProfile()
     : this("1", "User") {
    }

    public UserProfile(string id, string name) {
        Id = IsNotNullOrWhiteSpace(id);
        Name = IsNotNullOrWhiteSpace(name);
        Language = "English";
        Facts = [];
    }

    public string Id {
        get => (string)this[nameof(Id)];
        init => this[nameof(Id)] = value;
    }

    public string Name {
        get => (string)this[nameof(Name)];
        init => this[nameof(Name)] = value;
    }

    public string Language {
        get => (string)this[nameof(Language)];
        init => this[nameof(Language)] = value;
    }

    public List<string> Facts {
        get => (List<string>)this[nameof(Facts)];
        init => this[nameof(Facts)] = value;
    }

    public Result Validate(IContext? context = null) => Result.Success();
}
