namespace DotNetToolbox.AI.Agents;

public interface IMapper<TMapper, out TRequest, in TResponse>
    where TMapper : IMapper<TMapper, TRequest, TResponse>
    where TRequest : class, IChatRequest
    where TResponse : class, IChatResponse {
    static abstract TRequest CreateRequest(IMessages chat, World world, UserProfile userProfile, IAgent agent);
    static abstract Message GetResponseMessage(IMessages chat, TResponse response);
}
