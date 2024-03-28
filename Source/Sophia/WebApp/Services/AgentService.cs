using IHasMessages = Sophia.Models.Chats.IHasMessages;

namespace Sophia.WebApp.Services;

public class AgentService(IAgentFactory factory, IWorldService worldService, IUserService userService, IChatsService chatService)
    : AsyncResponseConsumer<AgentService>,
      IAgentService {

    public async Task<string> GetResponse(GetResponseRequest request) {
        try {
            var chat = await chatService.GetById(request.ChatId);
            if (chat is null) return "Error!";
            var agent = await CreateAgent(chat);
            var chatModel = chat.ToModel();
            var result = await agent.SendRequest(this, chatModel, request.AgentNumber);
            return result.IsOk ? chatModel.Messages[^1].AsText() : result.ToString();
        }
        catch (Exception ex) {
            throw new ArgumentException("Error creating agent.", ex);
        }
    }

    private async Task<IAgent> CreateAgent(ChatData chat) {
        var world = await worldService.GetWorld();
        var user = await userService.GetCurrentUserProfile();
        var selectedAgent = chat.Agents[0];
        var agent = factory.Create(selectedAgent.Options.Provider.Name);
        agent.World = world.ToModel();
        agent.User = user.ToModel();
        agent.Persona = selectedAgent.Persona.ToModel();
        agent.Model = selectedAgent.Options.ToModel();
        return agent;
    }

    protected override async Task OnResponseReceived(Guid chatId, int? agentNumber, Message message, CancellationToken ct) {
        try {
            var chat = await chatService.GetById(chatId)
                        ?? throw new ArgumentException("Chat not found.", nameof(chatId));
            var responseMessage = CreateMessage(chat, message);
            var agent = chat.Agents.FirstOrDefault(i => i.AgentNumber == agentNumber);
            agent?.Messages.Add(responseMessage);
            chat.Messages.Add(responseMessage);
        }
        catch (Exception ex) {
            throw new ArgumentException("Error creating agent.", ex);
        }
    }

    private static MessageData CreateMessage(IHasMessages parent, Message message)
        => new() {
            Type = "assistant",
            Index = parent.Messages.Count,
            Timestamp = DateTime.UtcNow,
            Content = message.AsText(),
        };
}
