namespace DotNetToolbox.AI.Chats;

public interface IChatHandlerFactory {
    IChatHandler Create(IChatOptions options);
}
