namespace BigMouth;

public class SayCommand : Command<SayCommand> {
    public SayCommand(IHasChildren parent)
        : base(parent, "Say", ["s"]) {
        AddOption("My");
        AddParameter("Info", "Color");
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) {
        var name = Application.Context["MyName"];
        Context.TryGetValue("My", out var type);
        type = type is string s && s.Equals("secret", StringComparison.OrdinalIgnoreCase) ? "Secret" : "Public";
        Context.TryGetValue("Info", out var info);
        Application.Context.TryGetValue($"{type}{info}", out var secret);
        Output.WriteLine(secret != null
                                                ? $"Ok {name}. Your {type} {info} is {secret}."
                                                : "I don't know.");
        return base.ExecuteAsync(ct);
    }
}
