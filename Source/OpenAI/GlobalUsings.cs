// Global using directives

global using System.Collections.Concurrent;
global using System.Diagnostics.CodeAnalysis;
global using System.Net;
global using System.Net.Http.Json;
global using System.Text.Json;
global using System.Text.Json.Serialization;

global using DotNetToolbox.Http;
global using DotNetToolbox.Http.Extensions;
global using DotNetToolbox.Http.Options;
global using DotNetToolbox.OpenAI.Chats;
global using DotNetToolbox.OpenAI.Chats.DataModels;
global using DotNetToolbox.OpenAI.HttpProvider;
global using DotNetToolbox.OpenAI.Models;
global using DotNetToolbox.OpenAI.Models.DataModels;
global using DotNetToolbox.OpenAI.Tools;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Logging.Abstractions;
global using Microsoft.Extensions.Options;

global using static DotNetToolbox.Ensure;

global using OpenAiModel = DotNetToolbox.OpenAI.Models.DataModels.Model;
global using Model = DotNetToolbox.OpenAI.Models.Model;
