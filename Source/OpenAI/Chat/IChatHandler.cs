namespace DotNetToolbox.OpenAI.Agents;

/// <summary>
/// This class contains methods for handling agent messages using OpenAI.
/// </summary>
public interface IChatHandler {
    /// <summary>
    /// Creates a new agent with the specified model and setup message.
    /// </summary>
    /// <param name="configure">A function to enable te configuration of the agent behavior.</param>
    /// <param name="stop"></param>
    /// <returns>The new agent.</returns>
    Task<Chat> Create(Action<AgentBuilder> configure, CancellationToken stop = default);

    /// <summary>
    /// Creates a new agent with the specified model and setup message.
    /// </summary>
    /// <param name="stop"></param>
    /// <returns>The new agent.</returns>
    Task<Chat> Create(CancellationToken stop = default);

    /// <summary>
    /// Sends a message to the specified agent and returns the response.
    /// </summary>
    /// <param name="agent">The ID of the agent to send the message to.</param>
    /// <param name="message">The message to send.</param>
    /// <param name="stop"></param>
    /// <returns>The response to the message.</returns>
    Task GetResponse(Chat agent, string message, CancellationToken stop = default);
}
