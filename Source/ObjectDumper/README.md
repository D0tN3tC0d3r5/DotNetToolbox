# DotNetToolbox Object Dumper

## Overview

The DotNetToolbox Object Dumper is a powerful and flexible library for .NET that allows developers to dump the contents of any .NET object in a human-readable format. It's a great tool for debugging and logging, providing a quick and easy way to visualize complex objects and data structures.

## Features

- **Flexible Formatting**: Choose from several layout options including JSON, Typed JSON, and Console.
- **Custom Formatters**: Define custom formatters for specific types to control how they are represented.
- **Depth Control**: Control the depth of nested objects and collections that are included in the output.
- **Full Type Information**: Option to include full type names in the output.
- **Culture-Specific Formatting**: Control the culture used for formatting values.

## Installation

The DotNet Toolbox Object Dumper is available as a NuGet package. To install it, run the following command in the Package Manager Console:

```shell
PM> Install-Package DotNetToolbox.ObjectDumper
```

## Usage

Here's a simple example of how to use the Object Dumper:

```csharp
var options = new DumpBuilderOptions
{
    Indented = true,
    ShowFullNames = true
};

var dumper = new DumpBuilder();
var result = dumper.Dump(myObject, options);

Console.WriteLine(result);
```

In this example, `myObject` is the object you want to dump, and `options` is an instance of `DumpBuilderOptions` that controls how the output is formatted.

## Documentation

For more detailed information on how to use the DotNet Toolbox Object Dumper, please refer to the [official documentation](#).

## Contributing

Contributions are welcome! Please read our [contributing guide](#) to learn about our development process, how to propose bugfixes and improvements, and how to build and test your changes to DotNet Toolbox Object Dumper.

## License

The DotNet Toolbox Object Dumper is licensed under the [MIT license](#).

## Contact

If you have any issues or feature requests, please [file an issue](#). For other questions or discussions, please join our [community forum](#).