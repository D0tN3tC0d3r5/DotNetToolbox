﻿namespace DotNetToolbox.AI.Jobs;

public class ChatJob : IJobStrategy<string[], string> {
    public string Instructions => "Engage in a conversation based on the provided messages.";

    public void AddPrompt(IChat chat, string[] input) {
        var isUser = true;
        foreach (var message in input) {
            var type = isUser ? MessageRole.User : MessageRole.Assistant;
            chat.Messages.Add(new(type, message));
            isUser = !isUser;
        }
    }

    public string GetResult(IChat chat) {
        var message = chat.Messages.Last(m => m.Role == MessageRole.Assistant);
        return message.Text;
    }
}
