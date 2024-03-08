namespace DotNetToolbox.AI.Chats;

public class Message(string source, Content[] parts) {
    public string Role { get; set; } = source;
    public Content[] Parts { get; set; } = parts;
}
