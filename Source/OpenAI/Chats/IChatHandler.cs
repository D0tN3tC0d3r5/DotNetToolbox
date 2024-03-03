namespace DotNetToolbox.OpenAI.Chats;

/// <summary>
/// This class contains methods for handling chat messages using OpenAI.
/// </summary>
public interface IChatHandler {
    /// <summary>
    /// Creates a new chat with the specified model and setup message.
    /// </summary>
    /// <param name="userName">Name used to identify the user.</param>
    /// <param name="configure">A function to enable te configuration of the chat behavior.</param>
    /// <param name="stop">A cancellation token.</param>
    /// <returns>The new chat.</returns>
    Task<Chat> Create(string userName, Action<ChatBuilder> configure, CancellationToken stop = default);

    /// <summary>
    /// Creates a new chat with the specified model and setup message.
    /// </summary>
    /// <param name="userName">Name used to identify the user.</param>
    /// <param name="stop">A cancellation token.</param>
    /// <returns>The new chat.</returns>
    Task<Chat> Create(string userName, CancellationToken stop = default);

    /// <summary>
    /// Sends a message from the user and expects a response.
    /// </summary>
    /// <param name="chat">The chat to send the message to.</param>
    /// <param name="message">The message to send.</param>
    /// <param name="stop">A cancellation token.</param>
    /// <returns>The response from the chat agent.</returns>
    Task<Message> SendUserMessage(Chat chat, string message, CancellationToken stop = default);

    /// <summary>
    /// Sends the result of a tool and expects a response.
    /// </summary>
    /// <param name="chat">The chat to send the message to.</param>
    /// <param name="results">The results from the tool calls.</param>
    /// <param name="stop">A cancellation token.</param>
    /// <returns>The response from the chat agent.</returns>
    Task<Message> SendToolResult(Chat chat, ToolResult[] results, CancellationToken stop = default);
}
