namespace DotNetToolbox.AI.Jobs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TaskResponseType {
    Markdown,
    SimpleText,
    Table,
    List,
    Json,
}
