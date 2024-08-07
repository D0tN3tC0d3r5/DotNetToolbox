namespace DotNetToolbox.AI.Jobs;

public class Instructions
    : Map<List<string>> {
    public Instructions() {
        Goals = [];
        Scope = [];
        Requirements = [];
        Assumptions = [];
        Constraints = [];
        Examples = [];
        Strategy = [];
        Validation = [];
    }

    public List<string> Goals {
        get => this[nameof(Goals)];
        init => this[nameof(Goals)] = value;
    }

    public List<string> Scope {
        get => this[nameof(Scope)];
        init => this[nameof(Scope)] = value;
    }

    public List<string> Requirements {
        get => this[nameof(Requirements)];
        init => this[nameof(Requirements)] = value;
    }

    public List<string> Assumptions {
        get => this[nameof(Assumptions)];
        init => this[nameof(Assumptions)] = value;
    }

    public List<string> Constraints {
        get => this[nameof(Constraints)];
        init => this[nameof(Constraints)] = value;
    }

    public List<string> Examples {
        get => this[nameof(Examples)];
        init => this[nameof(Examples)] = value;
    }

    public List<string> Strategy {
        get => this[nameof(Strategy)];
        init => this[nameof(Strategy)] = value;
    }

    public List<string> Validation {
        get => this[nameof(Validation)];
        init => this[nameof(Validation)] = value;
    }
}
