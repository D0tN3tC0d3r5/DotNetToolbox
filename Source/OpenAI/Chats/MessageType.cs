namespace DotNetToolbox.OpenAI.Chats;

public enum MessageType {
    System = 0,
    User = 1,
    Assistant = 2,
    Tool = 3,
    Error = 99,
}
