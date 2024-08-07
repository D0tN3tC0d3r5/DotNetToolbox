namespace DotNetToolbox;

[SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "Does not apply")]
// ReSharper disable once PossibleInterfaceMemberAmbiguity - Does not apply
public interface IContext
    : IMap {
    IServiceProvider Services { get; }
}
