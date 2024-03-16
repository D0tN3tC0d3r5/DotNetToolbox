namespace Sophia.ConsoleClient;

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

    public Persona LoadPersona(string name)
        => LoadFile<Persona>(_personasFolder, name);
    public AgentOptions LoadAgentOptions(string name)
        => LoadFile<AgentOptions>(_agentsFolder, name);
    public Skill LoadSkill(string name)
        => LoadFile<Skill>(_skillsFolder, name);

    public Chat LoadChat(string id)
        => LoadFile<Chat>(io.CombinePath(_chatsFolder, id), "chat");
    public void SaveChat(Chat chat)
        => SaveFile(chat, io.CombinePath(_chatsFolder, chat.Id), "chat");
    public void DeleteChat(string id)
        => DeleteFolder(io.CombinePath(_chatsFolder, id));

    private string[] GetFolders(string path)
        => io.GetFolders(path).ToArray();

    private void SaveFile<TData>(TData data, string path, string name)
        where TData : class {
        io.CreateFolder(path);
        using var file = GetFile(path, name);
        JsonSerializer.Serialize(file, data, _fileSerializationOptions);
    }

    private TData LoadFile<TData>(string path, string name)
        where TData : class {
        using var file = GetFile(path, name);
        var content = JsonSerializer.Deserialize<TData>(file, _fileSerializationOptions);
        return content ?? throw new InvalidOperationException($"Failed to load {name} from {path}.");
    }

    private void DeleteFolder(string path)
        => io.DeleteFolder(path, true);

    private Stream GetFile(string path, string name) {
        io.CreateFolder(path);
        var fileName = $"{path}/{name}.json";
        return io.OpenOrCreateFile(fileName);
    }
}
