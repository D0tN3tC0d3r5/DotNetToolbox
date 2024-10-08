﻿namespace DotNetToolbox.AI.OpenAI.Chats;

public class ChatRequestToolFunctionCallParameters(Dictionary<string, ChatRequestToolFunctionCallParameter>? properties, string[]? required) {
    [JsonPropertyName("properties")]
    public Dictionary<string, ChatRequestToolFunctionCallParameter>? Properties { get; set; } = properties;
    [JsonPropertyName("required")]
    public string[]? Required { get; set; } = required;
}
