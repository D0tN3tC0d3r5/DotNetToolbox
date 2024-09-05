namespace DotNetToolbox.AI.Chats;

public class Chat(string id, IContext context) : IChat {
    public Chat(Context context, IGuidProvider? guid = null)
        : this((guid ??= GuidProvider.Default).AsSortable.Create().ToString(), context) {
    }

    public string Id { get; } = id;
    public IContext Context { get; } = context;
    public List<Message> Messages { get; } = [];
    public uint TotalTokens { get; set; }
}
