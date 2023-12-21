# DotNet Toolbox Object Dumper

## Overview

The DotNet Toolbox Object Dumper is a powerful and flexible library for .NET that allows developers to dump the contents of any .NET object in a human-readable format. It's a great tool for debugging and logging, providing a quick and easy way to visualize complex objects and data structures.

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

The Object Dumper provides extension methods for any object. Here's a simple example of how to use the Object Dumper:

```csharp
var myObject = new MyClass();

// Dump object with default options
var result = myObject.Dump();

// Dump object with custom options
var result = myObject.Dump(options => {
    options.Layout = Layout.Json;
    options.Indented = true;
    options.ShowFullNames = true;
});

Console.WriteLine(result);
```

In this example, `myObject` is the object you want to dump. The `Dump` method is an extension method that dumps the object using the specified options. If no options are specified, it uses the default options.

You can also dump an object as JSON:

```csharp
var myObject = new MyClass();

// Dump object as JSON with default options
var result = myObject.DumpAsJson();

// Dump object as JSON with custom options
var result = myObject.DumpAsJson(options => {
    options.Indented = true;
    options.ShowFullNames = true;
});

Console.WriteLine(result);
```

## Documentation

For more detailed information on how to use the DotNet Toolbox Object Dumper, please refer to the [official documentation](#).

## Contributing

Contributions are welcome! Please read our [contributing guide](#) to learn about our development process, how to propose bugfixes and improvements, and how to build and test your changes to DotNet Toolbox Object Dumper.

## License

The DotNet Toolbox Object Dumper is licensed under the [MIT license](#).

## Contact

If you have any issues or feature requests, please [file an issue](#). For other questions or discussions, please join our [community forum](#).