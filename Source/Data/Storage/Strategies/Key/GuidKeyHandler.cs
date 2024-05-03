using DotNetToolbox.Environment;

namespace DotNetToolbox.Data.Strategies.Key;

public sealed class GuidKeyHandler(IGuidProvider? guid = null)
    : KeyHandler<Guid>(EqualityComparer<Guid>.Default) {
    private readonly IGuidProvider _guid = guid ?? GuidProvider.Default;

    public override Guid GetNext(string contextKey, Guid proposedKey) {
        lock (Lock)
            return proposedKey == Guid.Empty ? _guid.Create() : proposedKey;
    }
}
