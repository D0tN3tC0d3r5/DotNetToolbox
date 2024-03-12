namespace DotNetToolbox.Sophia;

public class FileRepository(IFileSystem io) {
    private const string _chatsFolder = "Chats";
    private const string _agentsFolder = "Agents";
    private const string _personasFolder = "Personas";
    private const string _skillsFolder = "Skills";

    private static readonly JsonSerializerOptions _fileSerializationOptions = new() {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
        IgnoreReadOnlyProperties = true,
    };

    public string[] ListChats()
        => GetFolders(_chatsFolder);

    public ValueTask<Persona> LoadPersona(string name, CancellationToken ct)
        => LoadFile<Persona>(_personasFolder, name, ct);
    public ValueTask<OpenAIAgentOptions> LoadAgentOptions(string name, CancellationToken ct)
        => LoadFile<OpenAIAgentOptions>(_agentsFolder, name, ct);
    public ValueTask<Skill> LoadSkill(string name, CancellationToken ct)
        => LoadFile<Skill>(_skillsFolder, name, ct);

    public ValueTask<Chat> LoadChat(string id, CancellationToken ct)
        => LoadFile<Chat>(io.CombinePath(_chatsFolder, id), "chat", ct);
    public Task SaveChat(Chat chat, CancellationToken ct)
        => SaveFile(chat, io.CombinePath(_chatsFolder, chat.Id), "chat", ct);
    public Task DeleteChat(string id, CancellationToken ct)
        => DeleteFolder(io.CombinePath(_chatsFolder, id), ct);

    private string[] GetFolders(string path)
        => io.GetFolders(path).ToArray();

    private async Task SaveFile<TData>(TData data, string path, string name, CancellationToken ct)
        where TData : class {
        io.CreateFolder(path);
        await using var file = GetFile(path, name);
        await JsonSerializer.SerializeAsync(file, data, _fileSerializationOptions, ct);
    }

    private async ValueTask<TData> LoadFile<TData>(string path, string name, CancellationToken ct)
        where TData : class {
        await using var file = GetFile(path, name);
        var content = await JsonSerializer.DeserializeAsync<TData>(file, _fileSerializationOptions, ct);
        return content ?? throw new InvalidOperationException($"Failed to load {name} from {path}.");
    }

    private Task DeleteFolder(string path, CancellationToken _) {
        io.DeleteFolder(path, true);
        return Task.CompletedTask;
    }

    private Stream GetFile(string path, string name) {
        io.CreateFolder(path);
        var fileName = $"{path}/{name}.json";
        return io.OpenOrCreateFile(fileName);
    }
}

public class StateMachine : IConsumer {
    public const uint Idle = 0;

    private bool _waitingResponse;

    private readonly IOutput _out;
    private readonly IPromptFactory _promptFactory;
    private readonly IApplication _app;
    private readonly IHttpClientProvider _httpClientProvider;
    private readonly MultipleChoicePrompt<uint> _mainMenu;
    private readonly World _world;
    private readonly ILogger<OpenAIQueuedAgent> _runnerLogger;

    private OpenAIQueuedAgent? _agent;
    private Chat? _chat;
    private readonly FileRepository _repository;

    public StateMachine(IApplication app, IHttpClientProvider httpClientProvider, ILoggerFactory loggerFactory) {
        _app = app;
        _httpClientProvider = httpClientProvider;
        _runnerLogger = loggerFactory.CreateLogger<OpenAIQueuedAgent>();
        _out = app.Environment.Output;
        _promptFactory = app.PromptFactory;
        _world = new(app.Environment);
        _mainMenu = _promptFactory.CreateMultipleChoiceQuestion("What do you want to do?", opt => {
            opt.AddOption(2, "Create a new chat", true);
            opt.AddOption(3, "Continue a existing chat", true);
            opt.AddOption(4, "Delete an existing chat", true);
            opt.AddOption(5, "Exit", true);
        });
        _repository = new(app.Environment.FileSystem);
    }

    public uint CurrentState { get; set; }
    public uint TotalTokens => _chat?.TotalTokens ?? 0;

    internal Task Start(uint initialState, CancellationToken ct) {
        CurrentState = initialState;
        return Process(string.Empty, ct);
    }

    internal Task Process(string input, CancellationToken ct)
        => CurrentState switch {
            1 => ShowMainMenu(ct),
            2 => Start(ct),
            3 => Resume(ct),
            4 => Finish(ct),
            5 => Exit(ct),
            6 => SendMessage(input, ct),
            _ => Task.CompletedTask,
        };

    private Task ShowMainMenu(CancellationToken ct) {
        CurrentState = _mainMenu.Ask();
        return Process(string.Empty, ct);
    }

    private async Task Start(CancellationToken ct) {
        try {
            var persona = await _repository.LoadPersona("TimeKeeper", ct);
            var options = await _repository.LoadAgentOptions("Fast", ct);
            _agent = new(_world, options, persona, _httpClientProvider, _runnerLogger);
            _agent.Run(ct);
            _chat = new(_app.Environment);
            await _repository.SaveChat(_chat, ct);
            _out.WriteLine("BackgroundAgent started.");
            CurrentState = Idle;
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
            throw;
        }
    }

    private Task Exit(CancellationToken _) {
        _app.Exit();
        return Task.CompletedTask;
    }

    private async Task Resume(CancellationToken ct) {
        var chatIds = _repository.ListChats();
        if (chatIds.Length == 0) {
            _out.WriteLine("No chat found.");
            CurrentState = 1;
            return;
        }

        var chatId = SelectChat("Select a chat to resume:", chatIds);
        if (chatId == string.Empty) {
            CurrentState = 1;
            return;
        }

        _chat = await _repository.LoadChat(chatId, ct);
        _out.WriteLine($"Resuming chat '{chatId}'.");
        CurrentState = Idle;
    }

    private async Task Finish(CancellationToken ct) {
        var chatIds = _repository.ListChats();
        if (chatIds.Length == 0) {
            _out.WriteLine("No chat found.");
            CurrentState = 1;
            return;
        }

        var chatId = SelectChat("Select a chat to delete permanently:", chatIds);
        if (chatId == string.Empty) {
            CurrentState = 1;
            return;
        }

        await _repository.DeleteChat(chatId, ct);
        _out.WriteLine($"chat '{chatId}' cancelled.");
        CurrentState = 1;
    }

    private string SelectChat(string question, IEnumerable<string> items) {
        var options = items.AsIndexed().ToList(i => new MultipleChoiceOption(i.Index, i.Value));
        options.Add(new((uint)options.Count, string.Empty, "Back to the main menu."));
        return _promptFactory.CreateMultipleChoiceQuestion<string>(question, options).Ask();
    }

    private async Task SendMessage(string input, CancellationToken ct) {
        if (_agent is null) {
            CurrentState = 1;
            return;
        }
        _out.Write("- ");
        _chat!.Messages.Add(new("user", input));
        _waitingResponse = true;
        await _repository.SaveChat(_chat, ct);
        await _agent.HandleRequest(this, _chat, ct);
        while (_waitingResponse) await Task.Delay(100, ct);
    }

    public async Task ProcessResponse(string chatId, Message response, CancellationToken ct) {
        if (_chat is null || _chat.Id != chatId) return;
        foreach (var part in response.Parts) _out.WriteLine(part.Value);
        await _repository.SaveChat(_chat, ct);
        _waitingResponse = false;
    }
}
