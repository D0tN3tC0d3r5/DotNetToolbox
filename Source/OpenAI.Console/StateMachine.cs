using DotNetToolbox.OpenAI.Agents;

namespace DotNetToolbox.Sophia;

public class StateMachine {
    private readonly IOutput _out;
    private readonly IFileSystem _io;
    private readonly IQuestionFactory _ask;
    private readonly IApplication _app;
    private readonly IAgentHandler _chatHandler;
    private const string _baseDataFolder = "Data";

    public StateMachine(IApplication app, IAgentHandler chatHandler) {
        _app = app;
        _chatHandler = chatHandler;
        _io = app.Environment.FileSystem;
        _out = app.Environment.Output;
        _ask = app.Ask;
        _io.CreateFolder(_baseDataFolder);
    }

    public Agent? CurrentChat { get; set; }
    public uint CurrentState { get; set; }

    internal void ShowMainMenu()
        => CurrentState = _ask.MultipleChoice("What do you want to do?", opt => {
            opt.AddChoice("Start a new chat");
            opt.AddChoice("Continue a existing chat");
            opt.AddChoice("Delete an existing chat");
            opt.AddChoice("Exit");
        }) + 1;

    internal void Start() => ShowMainMenu();

    internal Task Process(string input, CancellationToken ct) {
        switch (CurrentState) {
            case 0: ShowMainMenu(); break;
            case 1: return CreateChat(ct);
            case 2: return ResumeChat(ct);
            case 3: DeleteChat(); break;
            case 4: _app.Exit(); break;
            case 5: return GetResponse(input, ct);
        }

        return Task.CompletedTask;
    }

    internal async Task CreateChat(CancellationToken ct) {
        CurrentChat = await _chatHandler.Create(ct).ConfigureAwait(false);
        _io.CreateFolder($"{_baseDataFolder}/{CurrentChat.Id}");
        await using var chatFile = _io.OpenOrCreateFile($"{_baseDataFolder}/{CurrentChat.Id}/chat.json");
        await JsonSerializer.SerializeAsync(chatFile, CurrentChat, cancellationToken: ct);
        _out.WriteLine($"Chat session '{CurrentChat.Id}' started.");
        CurrentState = 5;
    }

    internal async Task ResumeChat(CancellationToken ct) {
        var folders = _io.GetFolders(_baseDataFolder).ToArray();
        if (folders.Length == 0) {
            _out.WriteLine("No chat sessions found.");
            CurrentState = 0;
            return;
        }
        var chatIndex = _ask.MultipleChoice("Select a chat session to resume:",
                                            opt => {
                                                foreach (var folder in folders) opt.AddChoice(folder);
                                            });
        var chatId = folders[chatIndex];
        await using var chatFile = _io.OpenFileAsReadOnly($"{_baseDataFolder}/{chatId}/chat.json");
        CurrentChat = await JsonSerializer.DeserializeAsync<Agent>(chatFile, cancellationToken: ct);
        _out.WriteLine($"Resuming chat session '{chatId}'.");
        CurrentState = 5;
    }

    internal void DeleteChat() {
        var folders = _io.GetFolders(_baseDataFolder).ToArray();
        if (folders.Length == 0) {
            _out.WriteLine("No chat sessions found.");
            CurrentState = 0;
            return;
        }
        var chatIndex = _ask.MultipleChoice("Select a chat session to resume:",
                                            opt => {
                                                foreach (var folder in folders) opt.AddChoice(folder);
                                            });
        var chatId = folders[chatIndex];
        _io.DeleteFolder($"Data/{chatId}", true);
        _out.WriteLine($"Chat session '{chatId}' deleted.");
        CurrentState = 0;
    }

    internal async Task GetResponse(string input, CancellationToken ct) {
        _out.Write("- ");
        await _chatHandler.GetResponse(CurrentChat!, input, ct);
        _out.WriteLine(CurrentChat!.Messages[^1].Content);
    }
}
