namespace DotNetToolbox.AI.Chats;

public interface IRoles {
    static abstract string System { get; }
    static abstract string User { get; }
    static abstract string Assistant { get; }
    static abstract string Tool { get; }
}
