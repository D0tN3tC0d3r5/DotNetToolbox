using DotNetToolbox.OpenAI.Chats;

namespace DotNetToolbox.OpenAI.DataModels;

internal record DeltaChoice : Choice {
    public required Message Delta { get; init; }
}
