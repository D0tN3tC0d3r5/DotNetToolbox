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
