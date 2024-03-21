﻿namespace Sophia.ConsoleClient;

public class StateMachine : IConsumer {
    public const uint Idle = 0;

    private bool _waitingResponse;

    private readonly IOutput _out;
    private readonly IPromptFactory _promptFactory;
    private readonly IApplication _app;
    private readonly MultipleChoicePrompt<uint> _mainMenu;

    private readonly StandardAgent? _agent;
    private Chat? _chat;
    private readonly FileRepository _repository;

    public StateMachine(IApplication app, IHttpClientProvider httpClientProvider, ILoggerFactory loggerFactory) {
        _app = app;
        _out = app.Environment.Output;
        _promptFactory = app.PromptFactory;
        _mainMenu = _promptFactory.CreateMultipleChoiceQuestion("What do you want to do?", opt => {
            opt.AddOption(2, "Create a new chat", true);
            opt.AddOption(3, "Continue a existing chat", true);
            opt.AddOption(4, "Delete an existing chat", true);
            opt.AddOption(5, "Exit", true);
        });

        _repository = new(app.Environment.FileSystem);

        var world = new World(app.Environment.DateTime);
        var persona = _repository.LoadPersona("TimeKeeper");
        var options = _repository.LoadAgentOptions("Fast");
        var factory = new AgentFactory(httpClientProvider, loggerFactory);
        _agent = factory.Create<StandardAgent>(world, options, persona);
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

    private Task Start(CancellationToken _) {
        try {
            _chat = new(_app.Environment);
            _repository.SaveChat(_chat);
            _out.WriteLine("New chat started.");
            CurrentState = Idle;
            return Task.CompletedTask;
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

    private Task Resume(CancellationToken _) {
        var chatIds = _repository.ListChats();
        if (chatIds.Length == 0) {
            _out.WriteLine("No chat found.");
            CurrentState = 1;
            return Task.CompletedTask;
        }

        var chatId = SelectChat("Select a chat to resume:", chatIds);
        if (chatId == string.Empty) {
            CurrentState = 1;
            return Task.CompletedTask;
        }

        _chat = _repository.LoadChat(chatId);
        _out.WriteLine($"Resuming chat '{chatId}'.");
        CurrentState = Idle;
        return Task.CompletedTask;
    }

    private Task Finish(CancellationToken _) {
        var chatIds = _repository.ListChats();
        if (chatIds.Length == 0) {
            _out.WriteLine("No chat found.");
            CurrentState = 1;
            return Task.CompletedTask;
        }

        var chatId = SelectChat("Select a chat to delete permanently:", chatIds);
        if (chatId == string.Empty) {
            CurrentState = 1;
            return Task.CompletedTask;
        }

        _repository.DeleteChat(chatId);
        _out.WriteLine($"chat '{chatId}' cancelled.");
        CurrentState = 1;
        return Task.CompletedTask;
    }

    private string SelectChat(string question, IEnumerable<string> items) {
        var options = items.AsIndexed().ToList(i => new MultipleChoiceOption(i.Index, i.Value));
        options.Add(new(options.Count, string.Empty, "Back to the main menu."));
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
        _repository.SaveChat(_chat);
        await _agent.HandleRequest(this, _chat, ct);
        while (_waitingResponse) await Task.Delay(100, ct);
        CurrentState = Idle;
    }

    public Task ProcessResponse(string chatId, Message response, CancellationToken _) {
        if (_chat is null || _chat.Id != chatId) return Task.CompletedTask;
        foreach (var part in response.Parts) _out.WriteLine(part.Value);
        _repository.SaveChat(_chat);
        _waitingResponse = false;
        return Task.CompletedTask;
    }
}
