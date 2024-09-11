namespace BigMouth;

public class SayCommand : Command<SayCommand> {
    public SayCommand(IHasChildren parent)
        : base(parent, "Say", ["s"]) {
        AddOption("My");
        AddParameter("Info", "Color");
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) {
        var name = Application.Map["MyName"];
        Map.TryGetValue("My", out var type);
        type = type is string s && s.Equals("secret", StringComparison.OrdinalIgnoreCase) ? "Secret" : "Public";
        Map.TryGetValue("Info", out var info);
        Application.Map.TryGetValue($"{type}{info}", out var secret);
        Output.WriteLine(secret != null
                                                ? $"Ok {name}. Your {type} {info} is {secret}."
                                                : "I don't know.");
        return base.ExecuteAsync(ct);
    }
}
