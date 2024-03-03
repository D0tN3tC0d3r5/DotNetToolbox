namespace DotNetToolbox.OpenAI.Chats;
public class Chat {
    public Chat(string userName, ChatOptions? options = null) {
        UserName = userName;
        Options = options ?? Options;
    }

    public string Id { get; } = Guid.NewGuid().ToString();
    public string UserName { get; }
    public ChatOptions Options { get; } = new();
    public List<Message> Messages { get; } = [];
    public int TotalNumberOfTokens { get; set; }
}
