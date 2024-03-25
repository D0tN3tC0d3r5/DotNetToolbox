namespace Sophia.WebApp.Services;

public class AgentService(IAgentFactory factory, IWorldService worlds, IChatsService chats)
    : AsyncResponseConsumer<AgentService>,
      IAgentService {

    public async Task<string> GetResponse(GetResponseRequest request) {
        try {
            var chat = await chats.GetById(request.ChatId);
            if (chat is null) return "Error!";
            var agent = await CreateAgent(chat);
            await agent.SendRequest(this, chat.ToModel());
            return chat.Messages[^1].Content;
        }
        catch (Exception ex) {
            throw new ArgumentException("Error creating agent.", ex);
        }
    }

    private async Task<IAgent> CreateAgent(ChatData chat) {
        var world = await worlds.GetWorld();
        var modelParts = chat.Agent.Model.Split(':');
        var agent = factory.Create<OpenAIAgent>(modelParts[0]);
        agent.World = world.ToModel();
        agent.Persona = chat.Agent.Persona.ToModel();
        agent.Options = chat.Agent.Options;
        return agent;
    }

    protected override async Task OnResponseReceived(string chatId, Message message, CancellationToken ct) {
        try {
            var chat = await chats.GetById(chatId)
                        ?? throw new ArgumentException("Chat not found.", nameof(chatId));
            chat.Messages.Add(new() {
                Type = "assistant",
                Index = chat.Messages.Count,
                Timestamp = DateTime.UtcNow,
                Content = message.AsText(),
            });
        }
        catch (Exception ex) {
            throw new ArgumentException("Error creating agent.", ex);
        }
    }
}
