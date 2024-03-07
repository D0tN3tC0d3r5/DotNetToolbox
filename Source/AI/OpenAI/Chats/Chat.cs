namespace DotNetToolbox.AI.OpenAI.Chats;

public class Chat(ChatOptions? options = null)
    : Chat<ChatOptions, Message>(options);
