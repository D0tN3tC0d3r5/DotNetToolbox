namespace DotNetToolbox.AI.Jobs;

public class Job(string id, JobContext context)
    : IJob {
    private readonly Chat _chat = new(id);

    public Job(IStringGuidProvider guid, JobContext context)
        : this(guid.CreateSortable(), context) {
    }
    public Job(JobContext context)
        : this(StringGuidProvider.Default, context) {
    }
    public string Id { get; } = id;
    public Dictionary<Type, Func<object, string>> Converters { get; } = [];

    public async Task<Result> Execute(CancellationToken ct) {
        SetSystemMessage();
        SetUserMessage();
        var response = await context.Agent.SendRequest(_chat, context, ct);
        if (!response.IsOk) return response.Errors;
        SetAgentResponse();
        return Result.Success();
    }

    private void SetSystemMessage() {
        if (_chat.Count != 0) return;
        var message = $"""
            # Agent Description
            {context.Persona.Prompt}

            # Task Description
            {context.Task.Prompt}
            """;
        _chat.AppendMessage(MessageRole.System, message);
    }

    private void SetUserMessage() {
        var message = JobInputHelper.FormatInput(context.Input, context.Task.InputTemplate, Converters);
        _chat.AppendMessage(MessageRole.User, message);
    }

    private void SetAgentResponse()
        => context.Output = JobOutputHelper.ExtractOutput(context.Task.ResponseType, _chat[^1]);
}
