﻿namespace DotNetToolbox.ConsoleApplication.Nodes.Arguments;

public interface IArgument : ILeaf {
    string Type { get; }
}
