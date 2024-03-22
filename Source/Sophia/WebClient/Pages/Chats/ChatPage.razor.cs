namespace Sophia.WebClient.Pages.Chats;

public partial class ChatPage {
    [Parameter] public int Id { get; set; }

    [Inject] public IChatsRemoteService ChatsService { get; set; } = default!;
    [Inject] public IAgentRemoteService AgentService { get; set; } = default!;
    [Inject] public required NavigationManager NavigationManager { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;

    private ChatData? _chat;
    private string _newMessage = string.Empty;
    private bool _isTyping;
    private string _errorMessage = string.Empty;
    private ElementReference _chatHistoryRef;

    protected override async Task OnInitializedAsync() {
        var chat = await ChatsService.GetById(Id);
        if (chat is null) {
            NavigationManager.NavigateTo("/chats");
            return;
        }
        _chat = chat;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (!firstRender) return;
        await ScrollToBottom();
    }

    private async Task Send() {
        try {
            if (string.IsNullOrWhiteSpace(_newMessage)) return;

            await SaveUserMessage(_newMessage);
            _newMessage = string.Empty;

            _isTyping = true;
            StateHasChanged();

            var request = new GetResponseRequest {
                ChatId = _chat!.Id,
                Message = _chat.Messages.Last().Content,
            };
            var agentResponse = await AgentService.GetResponse(request);
            await SaveAgentResponse(agentResponse);
        }
        catch (Exception ex) {
            _errorMessage = "An error occurred while sending the message.";
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally {
            _isTyping = false;
            StateHasChanged();
        }
    }

    private async Task HandleKeyDown(KeyboardEventArgs e) {
        if (e.Key != "Enter") return;
        await Send();
    }

    private Task SaveUserMessage(string userMessage)
        => SaveMessage(userMessage, "user");

    private Task SaveAgentResponse(string agentResponse)
        => SaveMessage(agentResponse, "assistant");

    private async Task SaveMessage(string messageContent, string type) {
        var message = new MessageData {
            Content = messageContent,
            Type = type,
            Timestamp = DateTime.UtcNow,
        };
        _chat!.Messages.Add(message);
        await ChatsService.AddMessage(_chat.Id, message);
        await ScrollToBottom();
    }

    private void GoBack()
        => NavigationManager.NavigateTo("/chats");

    private async Task ScrollToBottom()
        => await JsRuntime.InvokeVoidAsync("scrollToBottom", _chatHistoryRef);

    private async Task ScrollToTop()
        => await JsRuntime.InvokeVoidAsync("scrollToTop", _chatHistoryRef);
}
