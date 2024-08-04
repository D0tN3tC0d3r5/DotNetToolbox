namespace DotNetToolbox.AI.Jobs;

public class Instructions
    : Context<List<string>> {
    public Instructions() {
        this[nameof(Goals)] = [];
        this[nameof(Scope)] = [];
        this[nameof(Requirements)] = [];
        this[nameof(Assumptions)] = [];
        this[nameof(Constraints)] = [];
        this[nameof(Examples)] = [];
        this[nameof(Strategy)] = [];
        this[nameof(Validation)] = [];
    }

    public List<string> Goals => this[nameof(Goals)];
    public List<string> Scope => this[nameof(Scope)];
    public List<string> Requirements => this[nameof(Requirements)];
    public List<string> Assumptions => this[nameof(Assumptions)];
    public List<string> Constraints => this[nameof(Constraints)];
    public List<string> Examples => this[nameof(Examples)];
    public List<string> Strategy => this[nameof(Strategy)];
    public List<string> Validation => this[nameof(Validation)];
}
