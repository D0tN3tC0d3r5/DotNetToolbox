namespace DotNetToolbox.AI.Anthropic.Chats;

public class Chat(string userName, ChatOptions? options = null)
    : Chat<ChatOptions, Message>(userName, options);
