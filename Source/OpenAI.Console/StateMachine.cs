using System.Text.Json.Serialization;

using DotNetToolbox.Collections.Generic;

using DotNetToolbox.OpenAI.Agents;
using DotNetToolbox.OpenAI.Tools;

namespace DotNetToolbox.Sophia;

public class StateMachine {
    private readonly IOutput _out;
    private readonly IFileSystem _io;
    private readonly IQuestionFactory _ask;
    private readonly IApplication _app;
    private readonly IAgentHandler _missions;

    private static readonly JsonSerializerOptions _fileSerializationOptions = new() {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
        IgnoreReadOnlyProperties = true,
    };

    private const string _missionsFolder = "Missions";
    private const string _agentsFolder = "Agents";
    private const string _skillsFolder = "Skills";

    public StateMachine(IApplication app, IAgentHandler missions) {
        _app = app;
        _missions = missions;
        _io = app.Environment.FileSystem;
        _out = app.Environment.Output;
        _ask = app.Ask;
        _io.CreateFolder(_missionsFolder);
    }

    public Agent? Mission { get; set; }
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
            case 1: return CreateMission(ct);
            case 2: return ResumeMission(ct);
            case 3: CancelMission(); break;
            case 4: _app.Exit(); break;
            case 5: return GetResponse(input, ct);
        }

        return Task.CompletedTask;
    }

    internal async Task CreateMission(CancellationToken ct) {
        try {
            await using var agentFile = _io.OpenOrCreateFile($"{_agentsFolder}/TimeKeeper.json");
            var agent = JsonSerializer.Deserialize<Agents.Agent>(agentFile, _fileSerializationOptions)!;
            Mission = await _missions.Create(ct).ConfigureAwait(false);
            agent.Skills.ToList(s => {
                using var skillFile = _io.OpenOrCreateFile($"{_skillsFolder}/{s}.json");
                var skill = JsonSerializer.Deserialize<Skills.Skill>(skillFile, _fileSerializationOptions)!;
                return new Tool(new() {
                    Name = skill.Name,
                    Description = skill.Description,
                });
            }).ForEach(t => Mission.Options.Tools.Add(t));
            Mission.Messages[0].Name = agent.Name;
            Mission.Messages[0].Content = agent.Profile;
            _io.CreateFolder($"{_missionsFolder}/{Mission.Id}");
            await using var missionFile = _io.OpenOrCreateFile($"{_missionsFolder}/{Mission.Id}/agent.json");
            await JsonSerializer.SerializeAsync(missionFile, Mission, _fileSerializationOptions, ct);
            _out.WriteLine($"Agent '{Mission.Id}' started.");
            CurrentState = 99;
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
            throw;
        }
    }

    internal async Task ResumeMission(CancellationToken ct) {
        var folders = _io.GetFolders(_missionsFolder).ToArray();
        if (folders.Length == 0) {
            _out.WriteLine("No missions found.");
            CurrentState = 0;
            return;
        }
        var missionIndex = _ask.MultipleChoice("Select a mission to resume:",
                                            opt => {
                                                foreach (var folder in folders) opt.AddChoice(folder);
                                            });
        var missionId = folders[missionIndex];
        await using var agentFile = _io.OpenFileAsReadOnly($"{_missionsFolder}/{missionId}/agent.json");
        Mission = await JsonSerializer.DeserializeAsync<Agent>(agentFile, _fileSerializationOptions, cancellationToken: ct);
        _out.WriteLine($"Resuming mission '{missionId}'.");
        CurrentState = 5;
    }

    internal void CancelMission() {
        var folders = _io.GetFolders(_missionsFolder).ToArray();
        if (folders.Length == 0) {
            _out.WriteLine("No missions found.");
            CurrentState = 0;
            return;
        }
        var missionIndex = _ask.MultipleChoice("Select a mission to cancel:",
                                            opt => {
                                                foreach (var folder in folders) opt.AddChoice(folder);
                                            });
        var missionId = folders[missionIndex];
        _io.DeleteFolder($"{_missionsFolder}/{missionId}", true);
        _out.WriteLine($"mission '{missionId}' cancelled.");
        CurrentState = 0;
    }

    internal async Task GetResponse(string input, CancellationToken ct) {
        _out.Write("- ");
        await _missions.GetResponse(Mission!, input, ct);
        _out.WriteLine(Mission!.Messages[^1].Content);
        CurrentState = 99;
    }
}
