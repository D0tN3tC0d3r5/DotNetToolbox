﻿namespace DotNetToolbox.Graph.Builders;

public interface ICaseNodeBuilder {
    ICaseIsNodeBuilder Is(string key, Action<IWorkflowBuilder> setPath);
}
