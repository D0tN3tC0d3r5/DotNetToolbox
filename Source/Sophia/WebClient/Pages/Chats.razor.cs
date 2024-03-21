namespace Sophia.WebClient.Pages;

public partial class Chats {
    private IReadOnlyList<ChatData> _chats = [];
    private bool _showChatSetupDialog;
    private ChatData? _selectedChat;

    [Inject]
    public required IChatsRemoteService ChatsService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
        => _chats = await ChatsService.GetList();

    private void Start() {
        _selectedChat = new();
        _showChatSetupDialog = true;
    }

    private async Task StartChat() {
        if (_selectedChat!.Id == 0)
            await ChatsService.Start(_selectedChat);
        CloseChatDialog();
    }

    private void CloseChatDialog() {
        _showChatSetupDialog = false;
        _selectedChat = null;
    }

    private async Task Resume(int chatId) {
        await ChatsService.Archive(chatId);
        NavigationManager.NavigateTo($"/chat/{chatId}");
    }

    private async Task Archive(int chatId) {
        await ChatsService.Archive(chatId);
        _chats = await ChatsService.GetList();
    }

    private async Task Delete(int chatId) {
        await ChatsService.Delete(chatId);
        _chats = await ChatsService.GetList();
    }
}
