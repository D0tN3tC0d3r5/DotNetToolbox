global using System.Net;

global using DotNetToolbox.AI.OpenAI;
global using DotNetToolbox.AI.OpenAI.DataModels;
global using DotNetToolbox.AI.Utilities;
global using DotNetToolbox.Http;
global using DotNetToolbox.Results;
global using DotNetToolbox.TestUtilities.Logging;

global using FluentAssertions;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;

global using NSubstitute;
global using NSubstitute.ExceptionExtensions;

global using Xunit;

global using static System.Text.Json.JsonSerializer;

global using OpenAIModel = DotNetToolbox.AI.OpenAI.DataModels.Model;
