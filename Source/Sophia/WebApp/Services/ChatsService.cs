namespace Sophia.WebApp.Services;

public class ChatsService : IChatsService {
    // Implement the methods for managing chats
    // You can use a database or a storage service to persist the chats

    public Task<IReadOnlyList<ChatData>> GetList(string? filter = null) {
        // Retrieve the list of chats from storage
        // Return the list of chats
        return Task.FromResult<IReadOnlyList<ChatData>>(Array.Empty<ChatData>());
    }

    public Task Start(ChatData newChat) {
        // Create a new chat using the provided chat parameters
        // Save the new chat to storage
        // Return the newly created chat
        return Task.CompletedTask;
    }

    public Task<ChatData?> Resume(int chatId) {
        // Resume the chat with the specified ID
        // Update the chat's status in storage
        return Task.FromResult(new ChatData())!;
    }

    public Task Archive(int chatId) {
        // Archive the chat with the specified ID
        // Update the chat's status in storage
        return Task.CompletedTask;
    }

    public Task Delete(int chatId) {
        // Delete the chat with the specified ID from storage
        return Task.CompletedTask;
    }
}
