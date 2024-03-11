namespace DotNetToolbox.AI.Agents;

public interface IRequestHandler {
    CancellationTokenSource HandleRequest(IOriginator source, IChat chat);
}
