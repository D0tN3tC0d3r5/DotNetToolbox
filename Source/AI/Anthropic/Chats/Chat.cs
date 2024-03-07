namespace DotNetToolbox.AI.Anthropic.Chats;

public class Chat(ChatOptions? options = null)
    : Chat<ChatOptions, Message>(options);
