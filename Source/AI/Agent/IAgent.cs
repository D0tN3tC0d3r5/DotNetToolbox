namespace DotNetToolbox.AI.Agent;

public interface IAgent<TChat>
    where TChat : IChat {
    public Task Request();
}
