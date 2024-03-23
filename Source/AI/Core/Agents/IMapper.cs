
namespace DotNetToolbox.AI.Agents;

public interface IMapper {
    IChatRequest CreateRequest(IStandardAgent agent, IChat chat);
    Message CreateResponseMessage(IChat chat, IChatResponse response);
}
