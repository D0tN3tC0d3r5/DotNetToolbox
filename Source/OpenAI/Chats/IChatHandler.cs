namespace DotNetToolbox.OpenAI.Chats;

/// <summary>
/// This class contains methods for handling chat messages using OpenAI.
/// </summary>
public interface IChatHandler {
    /// <summary>
    /// Creates a new chat with the specified model and setup message.
    /// </summary>
    /// <param name="options">The options for the chat.</param>
    /// <returns>The ID of the new chat.</returns>
    Task<string> Create(ChatOptions options);

    /// <summary>
    /// Sends a message to the specified chat and returns the response.
    /// </summary>
    /// <param name="id">The ID of the chat to send the message to.</param>
    /// <param name="message">The message to send.</param>
    /// <returns>The response to the message.</returns>
    Task<string?> SendMessage(string id, string message);
}
