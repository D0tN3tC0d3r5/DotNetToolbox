namespace DotNetToolbox.AI.OpenAI.Chats;

public class Chat(string userName, ChatOptions? options = null)
    : Chat<ChatOptions, Message>(userName, options);
