namespace Sophia.WebApp.Services;

public class AgentService(IStandardAgentFactory factory, IWorldService worlds, IChatsService chats)
    : AsyncResponseConsumer<AgentService>,
      IAgentService {

    public async Task<string> GetResponse(GetResponseRequest request) {
        var chat = await chats.GetById(request.ChatId);
        if (chat is null) return "Error!";
        var agent = await CreateAgent(chat);
        await agent.SendRequest(this, chat.ToModel());
        return chat.Messages[^1].Content;
    }

    private async Task<IStandardAgent> CreateAgent(ChatData chat) {
        var world = (await worlds.GetWorld()).ToModel();
        var modelParts = chat.Agent.Model.Split(':');
        var persona = chat.Agent.Persona.ToModel();
        var options = chat.Agent.Options;
        var agent = factory.Create(modelParts[0], world, persona, options);
        return agent;
    }

    protected override async Task OnResponseReceived(string chatId, Message message, CancellationToken ct) {
        var chat = await chats.GetById(chatId)
                ?? throw new ArgumentException("Chat not found.", nameof(chatId));
        chat.Messages.Add(new() {
            Type = "assistant",
            Index = chat.Messages.Count,
            Timestamp = DateTime.UtcNow,
            Content = message.AsText(),
        });
    }
}
