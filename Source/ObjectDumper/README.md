## Object Dumper (DotNetToolbox.ObjectDumper)

### Introduction
DotNetToolbox.ObjectDumper is a powerful .NET 8 library for converting objects into a human-readable string representations. It provides customizable formatting options, including indentation, maximum depth control, and the ability to specify custom formatters for specific types.
This library is particularly useful for logging, debugging, and serialization scenarios where a visual representation of an object is needed.

### Table of Contents
1. [Installation](#installation)
2. [Dependencies](#dependencies)
3. [Key Features](#key-features)
4. [Extension Methods](#extension-methods)
5. [Object Dumping](#object-dumping)
6. [JSON Object Dumping](#json-object-dumping)
7. [Customization](#customization)

### Installation
The DotNet Toolbox Object Dumper is available as a NuGet package. To install it, run the following command in the Package Manager Console:

```shell
PM> Install-Package DotNetToolbox.ObjectDumper
```

### Dependencies
- .NET 8

### Key Features

- **Flexible Formatting**: Choose from several layout options including JSON, Typed JSON, and Console.
- **Custom Formatters**: Define custom formatters for specific types to control how they are represented.
- **Depth Control**: Control the depth of nested objects and collections that are included in the output.
- **Full Type Information**: Option to include full type names in the output.
- **Culture-Specific Formatting**: Control the culture used for formatting values.

### Extension Methods
The library extends any object with `Dump` and `DumpAsJson` methods, allowing any object to be dumped easily without requiring manual instantiation of dumper objects.

#### Examples:
1. **Dumping an object using default options:**
    ```csharp
    var person = new Person { Name = "John Doe", Age = 30 };
    Console.WriteLine(person.Dump());
    ```

2. **Customizing the dump output:**
    ```csharp
    Console.WriteLine(person.Dump(options => {
        options.IndentSize = 2;
        options.UseTabs = true;
        options.UseFullNames = true;
    });
    ```

3. **Dumping an object as JSON:**
    ```csharp
    Console.WriteLine(person.DumpAsJson());
    ```

4. **Customizing JSON dump options:**
    ```csharp
    Console.WriteLine(person.DumpAsJson(options => {
        options.Indented = false;
        options.MaxDepth = 5;
    }));
    ```

### Object Dumping
The core feature of DotNetToolbox.ObjectDumper is to create a human-readable string representation of an object. It traverses the object graph and outputs a formatted string that shows the types, properties, and values.

### JSON Object Dumping
In addition to the custom object dumping, the library also provides support for dumping objects as JSON strings using the `System.Text.Json` serializer, offering a familiar and standardized output.

### Customization
DotNetToolbox.ObjectDumper allows extensive customization of the output format:

- **DumpBuilderOptions**: Determines the behavior of the main object dumper. Users can configure indentation style, indent size, and whether to use full type names.
- **JsonDumpBuilderOptions**: Configures the JSON object dumper, with options for indentation and maximum object graph traversal depth.

One of the powerful features of `DumpBuilderOptions` is the ability to define custom formatters for types. This allows you to control exactly how objects of certain types are represented in the output.

#### Custom Formatter Example:
1. **Defining a custom formatter for a type:**
    ```csharp
    string customDump = person.Dump(options => {
        options.CustomFormatters[typeof(DateTime)] = value => ((DateTime)value).ToString("yyyy-MM-dd");
    });
    Console.WriteLine(customDump);
    ```

In this example, all `DateTime` objects within the `person` object will be formatted using the `"yyyy-MM-dd"` format.

