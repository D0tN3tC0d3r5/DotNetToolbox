using IHasMessages = Sophia.Models.Chats.IHasMessages;

namespace Sophia.WebApp.Services;

public class AgentService(IAgentFactory factory, IWorldService worlds, IChatsService chats)
    : AsyncResponseConsumer<AgentService>,
      IAgentService {

    public async Task<string> GetResponse(GetResponseRequest request) {
        try {
            var chat = await chats.GetById(request.ChatId);
            if (chat is null) return "Error!";
            var agent = await CreateAgent(chat);
            var chatModel = chat.ToModel();
            var result = await agent.SendRequest(this, chatModel, request.AgentNumber);
            if (result.IsOk) return chatModel.Messages[^1].AsText();
            return result.ToString();
        }
        catch (Exception ex) {
            throw new ArgumentException("Error creating agent.", ex);
        }
    }

    private async Task<IAgent> CreateAgent(ChatData chat) {
        var world = await worlds.GetWorld();
        var agent = factory.Create(chat.Agents[0].Provider.Name);
        agent.World = world.ToModel();
        agent.Persona = chat.Agents[0].Persona.ToModel();
        agent.Options = chat.Agents[0].Options;
        return agent;
    }

    protected override async Task OnResponseReceived(Guid chatId, int? agentNumber, Message message, CancellationToken ct) {
        try {
            var chat = await chats.GetById(chatId)
                        ?? throw new ArgumentException("Chat not found.", nameof(chatId));
            var responseMessage = CreateMessage(chat, chatId, agentNumber, message);
            var agent = chat.Agents.FirstOrDefault(i => i.Number == agentNumber);
            agent?.Messages.Add(responseMessage);
            chat.Messages.Add(responseMessage);
        }
        catch (Exception ex) {
            throw new ArgumentException("Error creating agent.", ex);
        }
    }

    private static MessageData CreateMessage(IHasMessages parent, Guid chatId, int? agentNumber, Message message)
        => new() {
            Type = "assistant",
            Index = parent.Messages.Count,
            Timestamp = DateTime.UtcNow,
            Content = message.AsText(),
        };
}
