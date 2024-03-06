// Global using directives

global using System.Collections.Concurrent;
global using System.Diagnostics.CodeAnalysis;
global using System.Net;
global using System.Net.Http.Json;
global using System.Text.Json;
global using System.Text.Json.Serialization;

global using DotNetToolbox.AI.Chats;
global using DotNetToolbox.AI.Models;
global using DotNetToolbox.AI.OpenAI;
global using DotNetToolbox.AI.OpenAI.DataModels;
global using DotNetToolbox.AI.Repositories;
global using DotNetToolbox.AI.Tools;
global using DotNetToolbox.Http;
global using DotNetToolbox.Http.Extensions;
global using DotNetToolbox.Http.Options;
global using DotNetToolbox.Results;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Logging.Abstractions;
global using Microsoft.Extensions.Options;

global using static DotNetToolbox.Ensure;

global using OpenAiModel = DotNetToolbox.AI.OpenAI.DataModels.Model;
