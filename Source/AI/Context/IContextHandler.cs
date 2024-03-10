using DotNetToolbox.AI.Agents;

namespace DotNetToolbox.AI.Context;
internal interface IContextHandler {
    World World { get; set; }
    Profile Profile { get; set; }
}
