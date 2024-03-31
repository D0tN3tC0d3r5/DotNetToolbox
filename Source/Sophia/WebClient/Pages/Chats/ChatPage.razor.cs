namespace Sophia.WebClient.Pages.Chats;

public partial class ChatPage {
    [Parameter] public string Id { get; set; } = default!;

    [Inject] public IChatsRemoteService ChatsService { get; set; } = default!;
    [Inject] public IAgentRemoteService AgentService { get; set; } = default!;
    [Inject] public required NavigationManager NavigationManager { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; } = default!;

    private ChatData _chat = default!;
    private string _newMessage = string.Empty;
    private bool _isTyping;
    private string _errorMessage = string.Empty;
    private ElementReference _chatHistoryRef;

    protected override async Task OnInitializedAsync() {
        _chat = (await ChatsService.GetById(Guid.Parse(Id)))!;
        if (_chat == null!)
            NavigationManager.NavigateTo("/chats");
    }

    protected override Task OnAfterRenderAsync(bool firstRender)
        => !firstRender
               ? Task.CompletedTask
               : ScrollToBottom();

    private async Task Send() {
        try {
            if (_chat.Messages.Count > 0 && _chat.Messages[^1].Type != "user" && string.IsNullOrWhiteSpace(_newMessage))
                return;
            if (!string.IsNullOrWhiteSpace(_newMessage))
                await SaveUserMessage(_newMessage);
            _newMessage = string.Empty;

            _isTyping = true;
            StateHasChanged();

            var request = new GetResponseRequest {
                ChatId = _chat.Id,
                Message = _chat.Messages.Last().Content,
            };
            var agentResponse = await AgentService.GetResponse(request);
            await SaveAgentResponse(agentResponse);
        }
        catch (Exception ex) {
            _errorMessage = $"An error occurred while sending the message. {ex}";
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally {
            _isTyping = false;
            StateHasChanged();
        }
    }

    private Task HandleKeyDown(KeyboardEventArgs e)
        => e.Key != "Enter"
               ? Task.CompletedTask
               : Send();

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
        _chat.Messages.Add(message);
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
