namespace DotNetToolbox.AI.Jobs;

public class UserProfile
    : Map,
      IValidatable {
    public UserProfile(string id) {
        Id = id;
        Name = string.Empty;
        Language = "English";
        Facts = [];
    }

    public required string Id {
        get => (string)this[nameof(Id)];
        init => this[nameof(Id)] = value;
    }

    public required string Name {
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
