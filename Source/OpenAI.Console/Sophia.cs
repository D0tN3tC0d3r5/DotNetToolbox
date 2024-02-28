using DotNetToolbox.OpenAI.Agents;

namespace DotNetToolbox.Sophia;

public class Sophia : ShellApplication<Sophia> {
    private readonly StateMachine _stateMachine;

    public Sophia(string[] args, IServiceProvider services, IAgentHandler chatHandler)
        : base(args, services) {
        AddCommand<StartChatCommand>();
        AddCommand<EndChatCommand>();
        _stateMachine = new(this, chatHandler);
    }

    protected override string GetPrePromptText() => $"[{TotalNumberOfTokens}] ";

    private int TotalNumberOfTokens => _stateMachine.CurrentChat?.TotalNumberOfTokens ?? 0;

    protected override async Task<Result> OnStart(CancellationToken ct) {
        _stateMachine.Start();
        await _stateMachine.Process(string.Empty, ct);
        return await base.OnStart(ct);
    }

    protected override async Task<Result> ExecuteDefault(CancellationToken ct) {
        await _stateMachine.Process(string.Empty, ct);
        return _stateMachine.CurrentState != 5
                   ? Result.Success()
                   : await base.ExecuteDefault(ct);
    }

    protected override async Task<Result> ProcessInput(string input, CancellationToken ct) {
        await _stateMachine.Process(input, ct);
        return await base.ProcessInput(input, ct);
    }

    public override void Exit(int code = IApplication.DefaultExitCode) {
        if (_stateMachine.CurrentState == 4) base.Exit(code);
        _stateMachine.CurrentState = 0;
    }
}
