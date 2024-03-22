namespace Sophia.WebClient.Pages.Chats;

public partial class ChatsPage {
    private IReadOnlyList<ChatData> _chats = [];
    private bool _showChatSetupDialog;
    private ChatData? _selectedChat;
    private bool _showDeleteConfirmationDialog;

    private bool _showArchived;
    private int _renamingChatId;
    private string _newChatName = string.Empty;

    [Inject] public required IChatsRemoteService ChatsService { get; set; }

    [Inject] public required NavigationManager NavigationManager { get; set; }

    protected override Task OnInitializedAsync()
        => Load();

    private async Task Load() {
        _chats = await ChatsService.GetList(_showArchived ? "ShowArchived" : null);
        StateHasChanged();
    }

    private void Start() {
        _selectedChat = new();
        _showChatSetupDialog = true;
    }

    private async Task StartChat() {
        if (_selectedChat!.Id == 0)
            await ChatsService.Create(_selectedChat);
        NavigationManager.NavigateTo($"/chat/{_selectedChat.Id}");
        CloseChatDialog();
    }

    private void CloseChatDialog() {
        _showChatSetupDialog = false;
        _selectedChat = null;
    }

    private void Resume(int chatId)
        => NavigationManager.NavigateTo($"/chat/{chatId}");

    private async Task Archive(int chatId) {
        await ChatsService.Archive(chatId);
        _chats = await ChatsService.GetList(_showArchived ? "ShowArchived" : null);
    }

    private async Task Unarchive(int chatId) {
        await ChatsService.Unarchive(chatId);
        _chats = await ChatsService.GetList(_showArchived ? "ShowArchived" : null);
    }

    private void Delete(ChatData chat) {
        _selectedChat = chat;
        _showDeleteConfirmationDialog = true;
    }

    private void CancelDelete() {
        _selectedChat = null;
        _showDeleteConfirmationDialog = false;
    }

    private async Task ExecuteDelete() {
        await ChatsService.Delete(_selectedChat!.Id);
        _chats = await ChatsService.GetList(_showArchived ? "ShowArchived" : null);
        _selectedChat = null;
        _showDeleteConfirmationDialog = false;
    }

    private void StartRename(int chatId, string currentName) {
        _renamingChatId = chatId;
        _newChatName = currentName;
    }

    private async Task ConfirmRename(int chatId) {
        await ChatsService.Rename(chatId, _newChatName);
        _chats = await ChatsService.GetList(_showArchived ? "ShowArchived" : null);
        CancelRename();
    }

    private void CancelRename() {
        _renamingChatId = 0;
        _newChatName = string.Empty;
    }
}
