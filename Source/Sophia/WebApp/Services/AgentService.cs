using DotNetToolbox.AI.Chats;

namespace Sophia.WebApp.Services;

public class AgentService(IStandardAgentFactory factory, IWorldService worlds, IChatsService chats, IProvidersService providers) : IAgentService {
    private bool _waitingResponse;
    private string _response = "Hello!";

    public async Task<string> GetResponse(GetResponseRequest request) {
        var world = (await worlds.GetWorld()).ToModel();
        var chat = await chats.GetById(request.ChatId);
        if (chat is null) return "Error!";
        var modelParts = chat.Agent.Model.Split(':');
        var persona = chat.Agent.Persona.ToModel();
        var options = chat.Agent.Options;
        var agent = factory.Create(modelParts[0], world, options, persona);
        var responseBuffer = new ResponseBuffer();
        var user = new UserProxy(chat, responseBuffer);
        var package = chat.ToModel();
        await agent.HandleRequest(user, package);
        while (!responseBuffer.ResponseReceived) await Task.Delay(100);
        return _response;
    }
}
