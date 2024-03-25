
namespace DotNetToolbox.AI.Agents;

public interface IMapper {
    IChatRequest CreateRequest(IAgent agent, IChat chat);
    Message CreateResponseMessage(IChat chat, IChatResponse response);
}
