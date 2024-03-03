using System.Globalization;

using DotNetToolbox.OpenAI.Chats;

namespace DotNetToolbox.Sophia;

public class StateMachine {
    private readonly IOutput _out;
    private readonly IFileSystem _io;
    private readonly IPromptFactory _promptFactory;
    private readonly IApplication _app;
    private readonly IChatHandler _missions;
    private readonly MultipleChoicePrompt _mainMenu;

    private static readonly JsonSerializerOptions _fileSerializationOptions = new() {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
        IgnoreReadOnlyProperties = true,
    };

    private const string _missionsFolder = "Missions";
    private const string _agentsFolder = "Agents";
    private const string _skillsFolder = "Skills";

    public StateMachine(IApplication app, IChatHandler missions) {
        _app = app;
        _missions = missions;
        _io = app.Environment.FileSystem;
        _out = app.Environment.Output;
        _promptFactory = app.PromptFactory;
        _io.CreateFolder(_missionsFolder);
        _mainMenu = _promptFactory.CreateMultipleChoiceQuestion("What do you want to do?", opt => {
            opt.AddChoice(2, "Start a new chat");
            opt.AddChoice(3, "Continue a existing chat");
            opt.AddChoice(4, "Delete an existing chat");
            opt.AddChoice(5, "Exit");
        });
    }

    public const uint Idle = 0;

    public Chat? Mission { get; set; }
    public uint CurrentState { get; set; }

    internal Task Start(uint initialState, CancellationToken ct) {
        CurrentState = initialState;
        return Process(string.Empty, ct);
    }

    internal Task Process(string input, CancellationToken ct) {
        switch (CurrentState) {
            case 1: return ShowMainMenu(ct);
            case 2: return CreateMission(ct);
            case 3: return ResumeMission(ct);
            case 4: CancelMission(); break;
            case 5: _app.Exit(); break;
            case 6: return SendMessage(input, ct);
        }

        return Task.CompletedTask;
    }

    private Task ShowMainMenu(CancellationToken ct) {
        CurrentState = _mainMenu.Ask();
        return Process(string.Empty, ct);
    }

    private async Task CreateMission(CancellationToken ct) {
        try {
            await using var agentFile = _io.OpenOrCreateFile($"{_agentsFolder}/TimeKeeper.json");
            var agent = JsonSerializer.Deserialize<Agent>(agentFile, _fileSerializationOptions)!;
            Mission = await _missions.Create("Argus", ct).ConfigureAwait(false);
            agent.Skills.ToList(s => {
                using var skillFile = _io.OpenOrCreateFile($"{_skillsFolder}/{s}.json");
                var skill = JsonSerializer.Deserialize<Skills.Skill>(skillFile, _fileSerializationOptions)!;
                var function = new Function {
                    Name = skill.Name,
                    Description = skill.Description,
                };
                return new Tool(function);
            }).ForEach(t => Mission.Options.Tools.Add(t));
            Mission.Messages[0].Name = agent.Name;
            Mission.Messages[0].Content = agent.Profile;
            _io.CreateFolder($"{_missionsFolder}/{Mission.Id}");
            await using var missionFile = _io.OpenOrCreateFile($"{_missionsFolder}/{Mission.Id}/agent.json");
            await JsonSerializer.SerializeAsync(missionFile, Mission, _fileSerializationOptions, ct);
            _out.WriteLine($"Chat '{Mission.Id}' started.");
            CurrentState = Idle;
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
            throw;
        }
    }

    private async Task ResumeMission(CancellationToken ct) {
        var folders = _io.GetFolders(_missionsFolder).ToArray();
        if (folders.Length == 0) {
            _out.WriteLine("No missions found.");
            CurrentState = 1;
            return;
        }

        var missionId = SelectMission("Select a mission to resume:", folders);
        if (missionId == string.Empty) {
            CurrentState = 1;
            return;
        }

        await using var agentFile = _io.OpenFileAsReadOnly($"{_missionsFolder}/{missionId}/agent.json");
        Mission = await JsonSerializer.DeserializeAsync<Chat>(agentFile, _fileSerializationOptions, cancellationToken: ct);
        _out.WriteLine($"Resuming mission '{missionId}'.");
        CurrentState = Idle;
    }

    private void CancelMission() {
        var folders = _io.GetFolders(_missionsFolder).ToArray();
        if (folders.Length == 0) {
            _out.WriteLine("No missions found.");
            CurrentState = 1;
            return;
        }

        var missionId = SelectMission("Select a mission to cancel:", folders);
        if (missionId == string.Empty) {
            CurrentState = 1;
            return;
        }

        _io.DeleteFolder($"{_missionsFolder}/{missionId}", true);
        _out.WriteLine($"mission '{missionId}' cancelled.");
        CurrentState = 1;
    }

    private string SelectMission(string question, string[] folders) {
        var missions = _promptFactory.CreateMultipleChoiceQuestion<string>(question, opt => {
            foreach (var folder in folders) opt.AddChoice(folder, folder);
            opt.AddChoice(string.Empty, "Back to the main menu.");
        });
        var missionId = missions.Ask();
        return missionId;
    }

    private async Task SendMessage(string input, CancellationToken ct) {
        _out.Write("- ");
        var response = await _missions.SendUserMessage(Mission!, input, ct);
        while (response.ToolCalls is not null) {
            var toolCalls = response.ToolCalls!;
            var results = toolCalls.ToArray(ExecuteTool);
            response = await _missions.SendToolResult(Mission!, results, ct);
        }
        _out.WriteLine(response.Content);
        CurrentState = Idle;
    }

    private static ToolResult ExecuteTool(ToolCall toolCall)
        => toolCall.Function.Name switch {
            "GetDateTime" => new(toolCall.Id, DateTime.Now.ToString(CultureInfo.InvariantCulture)),
            _ => throw new NotImplementedException(),
        };
}
