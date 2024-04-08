
namespace DotNetToolbox.AI.Agents;

public interface IMapper<TMapper, out TRequest, in TResponse>
    where TMapper : IMapper<TMapper, TRequest, TResponse>
    where TRequest : class, IChatRequest
    where TResponse : class, IChatResponse {
    static abstract TRequest CreateRequest(IChat chat, World world, UserProfile userProfile, IAgent agent);
    static abstract Message GetResponseMessage(IChat chat, TResponse response);
}
