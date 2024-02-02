using DotNetToolbox.ConsoleApplication.Nodes;
using DotNetToolbox.Results;

var app = BigMouth.Create(args, b => {
    b.AddAppSettings(); // This will add the values from appsettings.json to the context
    b.AddUserSecrets<Program>(); // This will add the values from the user secrets to the context
});

await app.RunAsync();

public class BigMouth
    : ShellApplication<BigMouth> {
    public BigMouth(string[] args, IServiceProvider services)
        : base(args, services) {
        AddCommand<SayCommand>();
    }

    protected override Task<Result> Execute(CancellationToken ct) {
        var name = Context["MyName"];
        Environment.Output.WriteLine($"Hello {name}.");
        return base.Execute(ct);
    }
}

public class SayCommand : Command<SayCommand> {
    public SayCommand(IHasChildren parent)
        : base(parent, "Say", ["s"]) {
        AddOption("My");
        AddParameter("Info", "Color");
    }

    public override Task<Result> Execute(CancellationToken ct = default) {
        var name = Application.Context["MyName"];
        Context.TryGetValue("My", out var type);
        type = type?.Equals("secret", StringComparison.CurrentCultureIgnoreCase) ?? false ? "Secret" : "Public";
        Context.TryGetValue("Info", out var info);
        Application.Context.TryGetValue($"{type}{info}", out var secret);
        Environment.Output.WriteLine(secret != null
                                         ? $"Ok {name}. Your {type} {info} is {secret}."
                                         : "I don't know.");
        return base.Execute(ct);
    }
}
