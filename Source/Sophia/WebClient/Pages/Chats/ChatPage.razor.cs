namespace Sophia.WebClient.Pages.Chats;

public partial class ChatPage {
    [Parameter] public int Id { get; set; }

    [Inject] public IChatsService ChatsService { get; set; } = default!;
    [Inject] public IAgentService AgentService { get; set; } = default!;
    [Inject] public required NavigationManager NavigationManager { get; set; }

    private ChatData _chat = default!;
    private string _newMessage = string.Empty;

    protected override async Task OnInitializedAsync() {
        var chat = await ChatsService.GetById(Id);
        if (chat is null) NavigationManager.NavigateTo("/chats");
        _chat = chat!;
    }

    private async Task Send() {
        try {
            if (string.IsNullOrWhiteSpace(_newMessage)) return;
            await SaveUserMessage(_newMessage);
            var agentResponse = await AgentService.GetResponse(_newMessage);
            await SaveAgentResponse(agentResponse);
        }
        finally {
            _newMessage = string.Empty;
        }
    }

    private Task SaveUserMessage(string userMessage)
        => SaveMessage(userMessage, false);

    private Task SaveAgentResponse(string agentResponse)
        => SaveMessage(agentResponse, false);

    private Task SaveMessage(string agentResponse, bool isUserMessage) {
        var message = new MessageData {
            Content = agentResponse,
            IsUserMessage = isUserMessage,
        };
        _chat!.Messages.Add(message);
        return ChatsService.AddMessage(_chat.Id, message);
    }
}
