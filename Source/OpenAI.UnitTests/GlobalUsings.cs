global using System.Net;

global using DotNetToolbox.OpenAI.Chats.DataModels;
global using DotNetToolbox.OpenAI.HttpProvider;
global using DotNetToolbox.OpenAI.Models.DataModels;
global using DotNetToolbox.OpenAI.Utilities;
global using DotNetToolbox.Results;
global using DotNetToolbox.TestUtilities.Logging;

global using FluentAssertions;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;

global using NSubstitute;
global using NSubstitute.ExceptionExtensions;

global using Xunit;

global using static System.Text.Json.JsonSerializer;

global using static DotNetToolbox.OpenAI.Chats.ChatOptions;

global using OpenAIModel = DotNetToolbox.OpenAI.Models.DataModels.Model;
