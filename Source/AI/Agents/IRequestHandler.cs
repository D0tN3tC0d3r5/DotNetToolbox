namespace DotNetToolbox.AI.Agents;

public interface IRequestHandler {
    CancellationTokenSource PostRequest(IOriginator source, IChat chat);
}
